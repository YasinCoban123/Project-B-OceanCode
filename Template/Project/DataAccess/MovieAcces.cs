using Microsoft.Data.Sqlite;
using Dapper;
public class MovieAcces
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private const string Table = "Movie";

    public void Write(MovieModel movie)
    {
        string sql = $"INSERT INTO {Table} (Title, GenreId, PGRating) VALUES (@Title, @GenreId, @PGRating)";
        _connection.Execute(sql, movie);
    }

    public List<MovieModel> GetAllMovie()
    {
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<MovieModel>(sql).ToList();
    }

    public void Delete(MovieModel movie)
    {
        string sql = $"DELETE FROM {Table} WHERE MovieId = @MovieId";
        _connection.Execute(sql, movie);
    }

    public bool ExistsByTitle(string title)
    {
        string sql = "SELECT COUNT(1) FROM Movie WHERE LOWER(Title) = LOWER(@Title)";
       return _connection.ExecuteScalar<int>(sql, new { Title = title }) > 0;
    }
}