using Microsoft.Data.Sqlite;
using Dapper;

public abstract class DataAccess
{
    protected SqliteConnection _connection =
        new SqliteConnection("Data Source=DataSources/project.db");

    protected abstract string Table { get; }
}
