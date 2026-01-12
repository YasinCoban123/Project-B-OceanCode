using Microsoft.Data.Sqlite;
using Dapper;

public class ReservedSeatAccess : DataAccess
{
    protected override string Table => "ReservedSeat";
    
    public bool IsSeatReserved(int seatId, int screeningId)
    {
        string sql = $"SELECT COUNT(1) FROM {Table} WHERE SeatId = @SeatId AND ScreeningId = @ScreeningId";
        int count = _connection.ExecuteScalar<int>(sql, new { SeatId = seatId, ScreeningId = screeningId });
        return count > 0;
    }
    
    public void AddReservedSeat(long reservationId, long seatId, long screeningId)
    {
        string sql = $"INSERT INTO {Table} (ReservationId, SeatId, ScreeningId) VALUES (@ReservationId, @SeatId, @ScreeningId)";
        _connection.Execute(sql, new { ReservationId = reservationId, SeatId = seatId, ScreeningId = screeningId });
    }
}