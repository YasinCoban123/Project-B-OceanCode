using Microsoft.Data.Sqlite;
using Dapper;
public class MovieAcces
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private const string Table = "Movie";

    public void Write(MovieModel movie)
    {
        string sql = $@"INSERT INTO {Table}(Title, GenreId, PGRating, Description, Actors, Duration)
        VALUES (@Title, @GenreId, @PGRating, @Description, @Actors, @Duration);";
        _connection.Execute(sql, movie);

    }

    public List<MovieModel> GetAllMovie()
    {
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<MovieModel>(sql).ToList();
    }

    public void Delete(MovieModel movie)
    {
        _connection.Execute("PRAGMA foreign_keys = OFF");
        _connection.Execute($"DELETE FROM {Table} WHERE MovieId = @MovieId", movie);
        _connection.Execute("PRAGMA foreign_keys = ON");
    }

    public bool ExistsByTitle(string title)
    {
        string sql = "SELECT COUNT(1) FROM Movie WHERE LOWER(Title) = LOWER(@Title)";
       return _connection.ExecuteScalar<int>(sql, new { Title = title }) > 0;
    }

    public void Update(MovieModel movie)
    {
        string sql = @"UPDATE Movie SET Title = @Title, GenreId = @GenreId, PGRating = @PGRating, Description = @Description, Actors = @Actors, Duration = @Duration
            WHERE MovieId = @MovieId";

        _connection.Execute(sql, movie);
    }

}