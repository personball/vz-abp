using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Volo.Abp.Domain.Services;
using Volo.Abp.Json;
using Volo.Abp.Timing;

namespace CompanyName.ProjectName.Npgsql;

public abstract class BatchImporterBase<TDbInfoProvider> : DomainService
    where TDbInfoProvider : class, INpgsqlDbInfoProvider, new()
{
    static BatchImporterBase()
    {
        DbInfoProvider = new TDbInfoProvider();
    }

    protected static TDbInfoProvider DbInfoProvider { get; private set; }

    protected IClock Clcok => LazyServiceProvider.LazyGetRequiredService<IClock>();

    protected IJsonSerializer JsonSerializer => LazyServiceProvider.LazyGetRequiredService<IJsonSerializer>();

    protected IConfiguration Configuration => LazyServiceProvider.LazyGetRequiredService<IConfiguration>();

    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(Configuration.GetConnectionString("Default"));
    }

    protected string BuildBinaryImportCommand(string tableName, NpgsqlDbField[] dbFields)
    {
        return $"COPY \"public\".\"{tableName}\" ({string.Join(", ", dbFields.Select(it => $"\"{it.Name}\""))}) FROM STDIN (FORMAT BINARY)";
    }

    protected async Task WithConnectionAsync(DbConnection dbConnection, Func<NpgsqlConnection, Task> p, CancellationToken cancellationToken)
    {
        if (dbConnection != null && dbConnection is NpgsqlConnection connection)
        {
            await p?.Invoke(connection);
        }
        else
        {
            await using (var connect = GetConnection())
            {
                await connect.OpenAsync(cancellationToken);

                await p?.Invoke(connect);
            }
        }
    }
}
