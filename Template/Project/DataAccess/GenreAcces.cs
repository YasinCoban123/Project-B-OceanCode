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

    public List<GenreModel> GetAllGenresObject()
    {
        string sql = $"SELECT * FROM {Table};";
        return _connection.Query<GenreModel>(sql).ToList();
    }

    public string GetMostPopularGenre()
    {
        string sql = @"SELECT Genre.Genre
          FROM Reservation
          JOIN Screening ON Reservation.ScreeningId = Screening.ScreeningId
          JOIN Movie ON Screening.MovieId = Movie.MovieId
          JOIN Genre ON Movie.GenreId = Genre.GenreId
          GROUP BY Genre.Genre
          ORDER BY COUNT(*) DESC
          LIMIT 1;";
          return _connection.QueryFirstOrDefault<string>(sql);
    }
}
