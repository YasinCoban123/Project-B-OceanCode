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

    public string GetMostPopularMovie()
    {
        string sql = @"SELECT Movie.Title
          FROM Reservation
          JOIN Screening ON Reservation.ScreeningId = Screening.ScreeningId
          JOIN Movie ON Screening.MovieId = Movie.MovieId
          GROUP BY Movie.Title
          ORDER BY COUNT(*) DESC
          LIMIT 1;";
    
        return _connection.QueryFirstOrDefault<string>(sql);
    }


        public long GetMovieDuration(long movieId)
    {
        string sql = "SELECT Duration FROM Movie WHERE MovieId = @MovieId";
        // retrieve as string and handle both numeric minutes and time formats like "1:28"
        string durationStr = _connection.ExecuteScalar<string>(sql, new { MovieId = movieId });

        if (string.IsNullOrWhiteSpace(durationStr))
            throw new FormatException($"Duration is null or empty for movie {movieId}.");

        if (long.TryParse(durationStr, out long minutes))
            return minutes;

        if (TimeSpan.TryParse(durationStr, out TimeSpan ts))
            return (long)ts.TotalMinutes;

        // try parsing H:mm without seconds (e.g. "1:28")
        var parts = durationStr.Split(':');
        if (parts.Length == 2 && int.TryParse(parts[0], out int h) && int.TryParse(parts[1], out int m))
            return h * 60L + m;

        throw new FormatException($"Cannot parse duration '{durationStr}' for movie {movieId}.");
    }
}