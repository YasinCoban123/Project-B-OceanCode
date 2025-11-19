using Microsoft.Data.Sqlite;
using Dapper;

public class HallAcces
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private string Table = "Hall";

    public List<HallModel> GetAllHalls()
    {
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<HallModel>(sql).ToList();;
    }
}