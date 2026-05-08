using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace CompanyName.ProjectName.Npgsql;

public static class NpgsqlBinaryImporterExtension
{
    private static readonly MethodInfo _writeMethod;

    static NpgsqlBinaryImporterExtension()
    {
        _writeMethod = typeof(NpgsqlBinaryImporter).GetMethods()
            .Where(it => it.IsGenericMethod)
            .Where(it => it.Name == nameof(NpgsqlBinaryImporter.WriteAsync))
            .Where(it =>
            {
                var parameters = it.GetParameters();
                if (parameters.Length != 3)
                {
                    return false;
                }

                if (parameters[1].ParameterType != typeof(string))
                {
                    return false;
                }

                return true;
            })
            .FirstOrDefault()
            ?? throw new Exception("writeMethod no found");
    }

    public static async Task WriteFieldValueAsync<T>(
        this NpgsqlBinaryImporter writer,
        NpgsqlDbField dbField,
        object source = null,
        CancellationToken cancellationToken = default)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        if (dbField == null)
        {
            throw new ArgumentNullException(nameof(dbField));
        }

        if (!dbField.HasValue)
        {
            await writer.WriteNullAsync();
            return;
        }

        var genericField = dbField.As<T>();

        if (genericField.HasConstValue)
        {
            await writer.WriteAsync(genericField.ConstValue, dbField.DbType);
            return;
        }

        var value = genericField.GetValue(source);
        if (value == null)
        {
            await writer.WriteNullAsync();
            return;
        }

        var nullableSourceType = Nullable.GetUnderlyingType(typeof(T));
        if (nullableSourceType == null)
        {
            await writer.WriteAsync(value, dbField.DbType);
            return;
        }

        await (Task)_writeMethod.MakeGenericMethod(nullableSourceType)
            .Invoke(
                writer,
                new[] { Convert.ChangeType(value, nullableSourceType), dbField.DbType, cancellationToken });
    }

    public static Task WriteFieldValueAsync<T>(
        this NpgsqlBinaryImporter writer,
        NpgsqlDbField dbField,
        T value,
        CancellationToken cancellationToken = default)
    {
        return WriteFieldValueAsync<T>(writer, dbField, (object)value, cancellationToken);
    }
}
