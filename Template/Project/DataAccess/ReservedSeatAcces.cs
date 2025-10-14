using Microsoft.Data.Sqlite;
using Dapper;

public class ReservedSeatAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private string Table = "ReservedSeat";

    public void AddReservedSeat(long reservationId, long seatId)
    {
        string sql = $"INSERT INTO {Table} (ReservationId, SeatId) VALUES (@ReservationId, @SeatId)";

        _connection.Execute(sql, new { ReservationId = reservationId, SeatId = seatId });
    }
}