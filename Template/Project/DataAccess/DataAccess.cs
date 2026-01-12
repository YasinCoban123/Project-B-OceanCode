using Microsoft.Data.Sqlite;
using Dapper;
using System.Runtime.InteropServices.Marshalling;
using System.Reflection.Metadata;
public class DataAcces
{
    protected SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    protected virtual string Table => "";
}
