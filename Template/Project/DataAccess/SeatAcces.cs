using Microsoft.Data.Sqlite;
using Dapper;

public class SeatAcces
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private string Table = "Seat";
    
    public List<(long SeatId, int RowNumber, int SeatNumber, string TypeName, decimal Price, bool IsTaken)> GetSeatStatusByScreening(long hallId, int screeningId)
    {
        string sql = @"
            SELECT 
                s.SeatId,
                s.RowNumber,
                s.SeatNumber,
                st.TypeName,
                st.Price,
                CAST(CASE WHEN rs.SeatId IS NULL THEN 0 ELSE 1 END AS BOOLEAN) AS IsTaken
            FROM Seat s
            JOIN SeatType st ON s.SeatTypeId = st.SeatTypeId
            LEFT JOIN ReservedSeat rs
                ON rs.SeatId = s.SeatId
               AND rs.ScreeningId = @ScreeningId
            WHERE s.HallId = @HallId
            ORDER BY s.RowNumber, s.SeatNumber;
        ";
    
        return _connection.Query<(long, int, int, string, decimal, bool)>(sql, new { HallId = hallId, ScreeningId = screeningId }).ToList();
    }

    public List<SeatModel> GetFreeSeatsForScreening(long hallId, int screeningId)
    {
        string sql = @"
            SELECT 
                s.SeatId,
                s.HallId,
                s.RowNumber,
                s.SeatNumber,
                s.SeatTypeId,
                st.TypeName AS SeatTypeName,
                st.Price AS SeatPrice
            FROM Seat s
            JOIN SeatType st ON s.SeatTypeId = st.SeatTypeId
            LEFT JOIN ReservedSeat rs
                ON rs.SeatId = s.SeatId
               AND rs.ScreeningId = @ScreeningId
            WHERE s.HallId = @HallId
              AND rs.SeatId IS NULL
            ORDER BY s.RowNumber, s.SeatNumber;
        ";

        return _connection.Query<SeatModel>(sql, new { HallId = hallId, ScreeningId = screeningId }).ToList();
    }
    
    public SeatModel? GetSeatById(long seatId)
    {
        string sql = $"SELECT * FROM {Table} WHERE SeatId = @SeatId";
        return _connection.QueryFirstOrDefault<SeatModel>(sql, new { SeatId = seatId });
    }

    
}