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

    public void Delete(long screeningId)
    {
        string sql = $"DELETE FROM {Table} WHERE ScreeningId = @ScreeningId";
        _connection.Execute(sql, new { ScreeningId = screeningId });
    }

    public List<string> GetScreenings()
    {
        string sql = $"SELECT ScreeningId, HallId, ScreeningStartingTime, Title, Genre, PGRating FROM {Table} JOIN Movie ON Screening.MovieId = Movie.MovieId;";
        return _connection.Query(sql)
            .Select(row => $"ScreeningId: {row.ScreeningId}, HallId: {row.HallId} StartTime: {row.ScreeningStartingTime}, Title: {row.Title}, Genre: {row.Genre}, PG: {row.PGRating}")
            .ToList();
    }

    public List<string> GetScreeningsByGenre(string genre)
    {
        string sql = @"
            SELECT ScreeningId, HallId, ScreeningStartingTime, Title, Genre, PGRating
            FROM Screening
            JOIN Movie ON Screening.MovieId = Movie.MovieId
            WHERE LOWER(Genre) = LOWER(@Genre)
            ORDER BY ScreeningStartingTime;";
    
        return _connection.Query(sql, new { Genre = genre })
            .Select(row => $"ScreeningId: {row.ScreeningId}, HallId: {row.HallId}, StartTime: {row.ScreeningStartingTime}, Title: {row.Title}, Genre: {row.Genre}, PG: {row.PGRating}")
            .ToList();
    }
    
    public List<string> GetScreeningsByDate()
    {
        string sql = @"
            SELECT ScreeningId, HallId, ScreeningStartingTime, Title, Genre, PGRating
            FROM Screening
            JOIN Movie ON Screening.MovieId = Movie.MovieId
            ORDER BY datetime(ScreeningStartingTime);";
    
        return _connection.Query(sql)
            .Select(row => $"ScreeningId: {row.ScreeningId}, HallId: {row.HallId}, StartTime: {row.ScreeningStartingTime}, Title: {row.Title}, Genre: {row.Genre}, PG: {row.PGRating}")
            .ToList();
    }

    public List<string> GetScreeningsByDay(string dayNumber)
    {
        string sql = @"
            SELECT ScreeningId, HallId, ScreeningStartingTime, Title, Genre, PGRating
            FROM Screening
            JOIN Movie ON Screening.MovieId = Movie.MovieId
            WHERE strftime('%w', ScreeningStartingTime) = @DayNumber
            ORDER BY datetime(ScreeningStartingTime);";

        return _connection.Query(sql, new { DayNumber = dayNumber })
            .Select(row => $"ScreeningId: {row.ScreeningId}, HallId: {row.HallId}, StartTime: {row.ScreeningStartingTime}, Title: {row.Title}, Genre: {row.Genre}, PG: {row.PGRating}")
            .ToList();
    }
}
