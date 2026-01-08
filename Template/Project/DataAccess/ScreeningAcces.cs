using Microsoft.Data.Sqlite;
using Dapper;

public class ScreeningAcces
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private string Table = "Screening";

    public void Add(ScreeningModel screening)
    {
        string sql = $"INSERT INTO {Table} (MovieId, HallId, ScreeningStartingTime) VALUES (@MovieId, @HallId, @StartTime)";
        _connection.Execute(sql, new { screening.MovieId, screening.HallId, StartTime = screening.ScreeningStartingTime });
    }

    public ScreeningModel? GetById(long screeningId)
    {
        string sql = $"SELECT * FROM {Table} WHERE ScreeningId = @ScreeningId";
        return _connection.QueryFirstOrDefault<ScreeningModel>(sql, new { ScreeningId = screeningId });
    }

    public List<ScreeningModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<ScreeningModel>(sql).ToList();
    }

    public void Update(ScreeningModel screening)
    {
        string sql = $"UPDATE {Table} SET MovieId = @MovieId, HallId = @HallId, ScreeningStartingTime = @StartTime WHERE ScreeningId = @ScreeningId";
        _connection.Execute(sql, new { screening.MovieId, screening.HallId, StartTime = screening.ScreeningStartingTime, screening.ScreeningId });
    }

    public void Delete(ScreeningModel screening)
    {
        _connection.Execute("PRAGMA foreign_keys = OFF");
        _connection.Execute($"DELETE FROM {Table} WHERE ScreeningId = @ScreeningId", screening);
        _connection.Execute("PRAGMA foreign_keys = ON");
    }

    public List<string> GetScreenings()
    {
        string sql = $@"
            SELECT ScreeningId, HallId, ScreeningStartingTime, Title, Genre.Genre AS Genre, PGRating, Description, Actors, Duration
            FROM {Table}
            JOIN Movie ON Screening.MovieId = Movie.MovieId
            JOIN Genre ON Movie.GenreId = Genre.GenreId;";
            
        return _connection.Query(sql)
            .Select(row => $"ScreeningId: {row.ScreeningId}, HallId: {row.HallId}, StartTime: {row.ScreeningStartingTime:dd-MM-yyyy HH:mm}, Title: {row.Title}, Genre: {row.Genre}, PG: {row.PGRating}, Description: {row.Description}, Actors: {row.Actors}, Duration: {row.Duration}")
            .ToList();
    }

    public List<string> GetScreeningsByGenre(string genre)
    {
        string sql = $@"
            SELECT ScreeningId, HallId, ScreeningStartingTime, Title, Genre.Genre AS Genre, PGRating, Description, Actors, Duration
            FROM {Table}
            JOIN Movie ON Screening.MovieId = Movie.MovieId
            JOIN Genre ON Movie.GenreId = Genre.GenreId
            WHERE LOWER(Genre.Genre) = LOWER(@Genre)
            ORDER BY ScreeningStartingTime;";


        return _connection.Query(sql, new { Genre = genre })
            .Select(row => $"ScreeningId: {row.ScreeningId}, HallId: {row.HallId}, StartTime: {row.ScreeningStartingTime:dd-MM-yyyy HH:mm}, Title: {row.Title}, Genre: {row.Genre}, PG: {row.PGRating}, Description: {row.Description}, Actors: {row.Actors}, Duration: {row.Duration}")
            .ToList();
    }

    public List<string> GetScreeningsByDate()
    {
        string sql = $@"
            SELECT ScreeningId, HallId, ScreeningStartingTime, Title, Genre.Genre AS Genre, PGRating, Description, Actors, Duration
            FROM {Table}
            JOIN Movie ON Screening.MovieId = Movie.MovieId
            JOIN Genre ON Movie.GenreId = Genre.GenreId
            ORDER BY datetime(ScreeningStartingTime);";

        return _connection.Query(sql)
            .Select(row => $"ScreeningId: {row.ScreeningId}, HallId: {row.HallId}, StartTime: {row.ScreeningStartingTime:dd-MM-yyyy HH:mm}, Title: {row.Title}, Genre: {row.Genre}, PG: {row.PGRating}, Description: {row.Description}, Actors: {row.Actors}, Duration: {row.Duration}")
            .ToList();
    }

    public List<string> GetScreeningsByDay(string dayNumber)
    {
        string sql = $@"
            SELECT ScreeningId, HallId, ScreeningStartingTime, Title, Genre.Genre AS Genre, PGRating, Description, Actors, Duration
            FROM {Table}
            JOIN Movie ON Screening.MovieId = Movie.MovieId
            JOIN Genre ON Movie.GenreId = Genre.GenreId
            WHERE strftime(
                '%w',
                substr(ScreeningStartingTime, 7, 4) || '-' ||
                substr(ScreeningStartingTime, 4, 2) || '-' ||
                substr(ScreeningStartingTime, 1, 2)
            ) = @DayNumber
            ORDER BY ScreeningStartingTime;";
    
        return _connection.Query(sql, new { DayNumber = dayNumber })
            .Select(row => $"ScreeningId: {row.ScreeningId}, HallId: {row.HallId}, StartTime: {row.ScreeningStartingTime:dd-MM-yyyy HH:mm}, Title: {row.Title}, Genre: {row.Genre}, PG: {row.PGRating}, Description: {row.Description}, Actors: {row.Actors}, Duration: {row.Duration}")
            .ToList();
    }


    public long GetPGRatingByScreeningId(long screeningId)
    {
        string sql = $@"
            SELECT Movie.PGRating
            FROM {Table}
            JOIN Movie ON Screening.MovieId = Movie.MovieId
            WHERE Screening.ScreeningId = @ScreeningId;";

        return _connection.QueryFirstOrDefault<long>(sql, new { ScreeningId = screeningId });
    }

    public DateTime GetScreeningStartTime(long screeningId)
    {
        string sql = $@"
            SELECT ScreeningStartingTime
            FROM {Table}
            WHERE ScreeningId = @ScreeningId;";

        string startTimeStr = _connection.QueryFirstOrDefault<string>(sql, new { ScreeningId = screeningId });
        return DateTime.Parse(startTimeStr);
    }

    public int GetHallIdByScreeningId(long screeningId)
    {
        string sql = $@"
            SELECT HallId
            FROM {Table}
            WHERE ScreeningId = @ScreeningId;";

        return _connection.QueryFirstOrDefault<int>(sql, new { ScreeningId = screeningId });
    }


}
