using Microsoft.Data.Sqlite;
using Dapper;

public class SeatAcces
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private string Table = "Seat";

    public SeatModel GetFirstEmptySeat(long hallId)
    {
        string sql = $"SELECT * FROM {Table} WHERE HallId = @HallId AND IsReserved = 0 LIMIT 1";
        return _connection.QueryFirstOrDefault<SeatModel>(sql, new { HallId = hallId });
    }

    public void MarkSeatAsReserved(long seatId)
    {
        string sql = $"UPDATE Seat SET IsReserved = 1 WHERE SeatId = @SeatId";
        _connection.Execute(sql, new { SeatId = seatId });
    }

}