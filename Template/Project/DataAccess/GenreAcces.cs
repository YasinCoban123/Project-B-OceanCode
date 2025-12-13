using Microsoft.Data.Sqlite;
using Dapper;
public class GenreAcces
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private const string Table = "Genre";

    public List<string> GetAllGenres()
    {
        string sql = $"SELECT Genre FROM {Table};";
        return _connection.Query<string>(sql).ToList();
    }
}
