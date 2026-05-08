using System;

namespace CompanyName.ProjectName.Npgsql;

public abstract class NpgsqlDbField
{
    protected NpgsqlDbField(string name, string dbType, bool hasValue = true)
    {
        Name = name;
        DbType = dbType;
        HasValue = hasValue;
    }

    public string Name { get; }

    public string DbType { get; }

    public bool HasValue { get; }

    public NpgsqlDbField<T> As<T>()
    {
        if (this is NpgsqlDbField<T> genericInstance)
        {
            return genericInstance;
        }

        throw new InvalidCastException(
            $"cannot cast \"{GetType().Name}\" to \"{nameof(NpgsqlDbField)}<{typeof(T).Name}>\" (at {Name} ({DbType}))");
    }
}

public abstract class NpgsqlDbField<T> : NpgsqlDbField
{
    protected NpgsqlDbField(
        string name, string dbType, bool hasValue = true)
        : base(name, dbType, hasValue)
    {
    }

    public bool HasConstValue { get; private set; }

    public T ConstValue { get; private set; }

    public abstract T GetValue(object source);

    protected NpgsqlDbField<T> AsConst(T value)
    {
        HasConstValue = true;
        ConstValue = value;

        return this;
    }
}

internal sealed class UuidNpgsqlDbField : NpgsqlDbField<Guid>
{
    public UuidNpgsqlDbField(string name)
        : base(name, "uuid")
    {
    }

    public override Guid GetValue(object source)
    {
        if (source is Guid value)
        {
            return value;
        }

        throw new NpgsqlDbFieldSourceValueNullException(this);
    }
}

internal sealed class NullableUuidNpgsqlDbField : NpgsqlDbField<Guid?>
{
    public NullableUuidNpgsqlDbField(string name, bool hasValue = true)
        : base(name, "uuid", hasValue)
    {
    }

    public override Guid? GetValue(object source)
    {
        if (source is Guid value)
        {
            return value;
        }

        return null;
    }
}

internal sealed class TimestampNpgsqlDbField : NpgsqlDbField<DateTime>
{
    public TimestampNpgsqlDbField(string name)
        : base(name, "timestamp")
    {
    }

    public static NpgsqlDbField<DateTime> Const(string name, DateTime constValue)
        => new TimestampNpgsqlDbField(name).AsConst(constValue);

    public override DateTime GetValue(object source)
    {
        if (source is DateTime value)
        {
            return value;
        }

        throw new NpgsqlDbFieldSourceValueNullException(this);
    }
}

internal sealed class NullableTimestampNpgsqlDbField : NpgsqlDbField<DateTime?>
{
    public NullableTimestampNpgsqlDbField(string name, bool hasValue = true)
        : base(name, "timestamp", hasValue)
    {
    }

    public override DateTime? GetValue(object source)
    {
        if (source is DateTime value)
        {
            return value;
        }

        return null;
    }
}

internal sealed class BooleanNpgsqlDbField : NpgsqlDbField<bool>
{
    public BooleanNpgsqlDbField(string name)
        : base(name, "boolean")
    {
    }

    public static NpgsqlDbField<bool> Const(string name, bool constValue)
        => new BooleanNpgsqlDbField(name).AsConst(constValue);

    public override bool GetValue(object source)
    {
        if (source is bool value)
        {
            return value;
        }

        throw new NpgsqlDbFieldSourceValueNullException(this);
    }
}

internal sealed class NullableBooleanNpgsqlDbField : NpgsqlDbField<bool?>
{
    public NullableBooleanNpgsqlDbField(string name, bool hasValue = true)
        : base(name, "boolean", hasValue)
    {
    }

    public override bool? GetValue(object source)
    {
        if (source is bool value)
        {
            return value;
        }

        return null;
    }
}

internal sealed class VarcharNpgsqlDbField : NpgsqlDbField<string>
{
    public VarcharNpgsqlDbField(string name, bool hasValue = true)
        : base(name, "character varying", hasValue)
    {
    }

    public override string GetValue(object source)
    {
        if (source == null)
        {
            return null;
        }

        if (source is string value)
        {
            return value;
        }

        return source.ToString();
    }
}

internal sealed class JsonbNpgsqlDbField<T> : NpgsqlDbField<string>
{
    public JsonbNpgsqlDbField(string name, bool hasValue = true)
        : base(name, "jsonb", hasValue)
    {
    }

    public override string GetValue(object source)
    {
        if (source == null)
        {
            return null;
        }

        if (source is string value)
        {
            return value;
        }

        return source.ToString();
    }
}

internal sealed class NumericNpgsqlDbField : NpgsqlDbField<decimal>
{
    public NumericNpgsqlDbField(string name)
        : base(name, "numeric")
    {
    }

    public override decimal GetValue(object source)
    {
        if (source == null)
        {
            throw new NpgsqlDbFieldSourceValueNullException(this);
        }

        if (source is int intValue)
        {
            return intValue;
        }
        else if (source is long longValue)
        {
            return longValue;
        }
        else if (source is byte byteValue)
        {
            return byteValue;
        }
        else if (source is float floatValue)
        {
            return checked((decimal)floatValue);
        }
        else if (source is double doubleValue)
        {
            return checked((decimal)doubleValue);
        }
        else if (source is decimal decimalValue)
        {
            return decimalValue;
        }

        throw new NpgsqlDbFieldSourceValueNullException(this);
    }
}

internal sealed class NullableNumericNpgsqlDbField : NpgsqlDbField<decimal?>
{
    public NullableNumericNpgsqlDbField(string name, bool hasValue = true)
        : base(name, "numeric", hasValue)
    {
    }

    public override decimal? GetValue(object source)
    {
        if (source == null)
        {
            return null;
        }

        if (source is int intValue)
        {
            return intValue;
        }
        else if (source is long longValue)
        {
            return longValue;
        }
        else if (source is byte byteValue)
        {
            return byteValue;
        }
        else if (source is float floatValue)
        {
            return checked((decimal)floatValue);
        }
        else if (source is double doubleValue)
        {
            return checked((decimal)doubleValue);
        }
        else if (source is decimal decimalValue)
        {
            return decimalValue;
        }

        return null;
    }
}

internal sealed class IntegerNpgsqlDbField : NpgsqlDbField<int>
{
    public IntegerNpgsqlDbField(string name)
        : base(name, "integer")
    {
    }

    public override int GetValue(object source)
    {
        if (source == null)
        {
            throw new NpgsqlDbFieldSourceValueNullException(this);
        }

        if (source is int intValue)
        {
            return intValue;
        }
        else if (source is byte byteValue)
        {
            return byteValue;
        }

        throw new NpgsqlDbFieldSourceValueNullException(this);
    }
}

internal sealed class NullableIntegerNpgsqlDbField : NpgsqlDbField<int?>
{
    public NullableIntegerNpgsqlDbField(string name, bool hasValue = true)
        : base(name, "integer", hasValue)
    {
    }

    public override int? GetValue(object source)
    {
        if (source == null)
        {
            return null;
        }

        if (source is int intValue)
        {
            return intValue;
        }
        else if (source is byte byteValue)
        {
            return byteValue;
        }

        return null;
    }
}

internal class NpgsqlDbFieldSourceValueNullException : ArgumentException
{
    public NpgsqlDbFieldSourceValueNullException(NpgsqlDbField source)
        : base($"source value required but was null: \"{source.Name}\" in \"{source.GetType().Name}\"")
    {
    }
}
