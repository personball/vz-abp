using System.Collections.Generic;

namespace CompanyName.ProjectName.Npgsql;

public interface INpgsqlDbInfoProvider
{
    string TableName { get; }

    IEnumerable<NpgsqlDbField> DbFields { get; }
}
