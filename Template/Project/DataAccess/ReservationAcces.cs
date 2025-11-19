using Microsoft.Data.Sqlite;
using Dapper;

public class ReservationAcces
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private string Table = "Reservation";

    public long AddReservation(ReservationModel reservation)
    {
        string sql = $"INSERT INTO {Table} (AccountId, ScreeningId, ReservationTime) VALUES (@AccountId, @ScreeningId, @ReservationTime)";

        _connection.Execute(sql, reservation);

        // auto increment vast zetten
        return _connection.QuerySingle<long>("SELECT last_insert_rowid()");
    }


    public List<ReservationModel> GetByAccountId(long accountId)
    {
        string sql = @"
        SELECT 
            reservation.ReservationId AS ReservationId,
            movie.Title AS MovieTitle,
            screening.ScreeningStartingTime AS ScreeningStartingTime,
            seat.RowNumber AS RowNumber,
            seat.SeatNumber AS SeatNumber
        FROM Reservation reservation
        JOIN Screening screening       ON reservation.ScreeningId = screening.ScreeningId
        JOIN Movie movie               ON screening.MovieId = movie.MovieId
        JOIN ReservedSeat reservedSeat ON reservation.ReservationId = reservedSeat.ReservationId
        JOIN Seat seat                 ON reservedSeat.SeatId = seat.SeatId
        WHERE reservation.AccountId = @AccountId
        ORDER BY screening.ScreeningStartingTime DESC;
        ";

        return _connection.Query<ReservationModel>(sql, new { AccountId = accountId }).ToList();
    }


    public bool Exists(long reservationId)
    {
        string sql = $"SELECT COUNT(1) FROM {Table} WHERE ReservationId = @ReservationId";
        return _connection.ExecuteScalar<int>(sql, new { ReservationId = reservationId }) > 0;
    }

    public void Delete(long reservationId)
    {
        string deleteSeats = "DELETE FROM ReservedSeat WHERE ReservationId = @ReservationId";
        int seatsDeleted = _connection.Execute(deleteSeats, new { ReservationId = reservationId });
        // Console.WriteLine($"[ReservationAcces.Delete] Deleted {seatsDeleted} rows from ReservedSeat for ReservationId={reservationId}");

        string deleteReservation = "DELETE FROM Reservation WHERE ReservationId = @ReservationId";
        int reservationsDeleted = _connection.Execute(deleteReservation, new { ReservationId = reservationId });
        // Console.WriteLine($"[ReservationAcces.Delete] Deleted {reservationsDeleted} rows from Reservation for ReservationId={reservationId}");

        if (seatsDeleted == 0 && reservationsDeleted == 0)
        {
            Console.WriteLine($"[ReservationAcces.Delete] No rows matched ReservationId={reservationId} - check DB file & schema.");
        }
    }


    public int GetSeatCountForUserScreening(long accountId, int screeningId)
    {
        string sql = @"
        SELECT COUNT(*)
        FROM Reservation r
        JOIN ReservedSeat rs ON r.ReservationId = rs.ReservationId
        WHERE r.AccountId = @AccountId
        AND r.ScreeningId = @ScreeningId;
        ";
        return _connection.ExecuteScalar<int>(sql, new { AccountId = accountId, ScreeningId = screeningId });
    }

    public List<ReservationModel> GetAllReservations()
    {
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<ReservationModel>(sql).ToList();
    }
}