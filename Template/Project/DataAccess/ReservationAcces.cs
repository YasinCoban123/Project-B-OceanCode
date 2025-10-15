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

    public IEnumerable<ReservationModel> GetByAccountId(long accountId)
    {
        string sql = $"SELECT * FROM {Table} WHERE AccountId = @AccountId";
        return _connection.Query<ReservationModel>(sql, new { AccountId = accountId });
    }

    public bool Exists(long reservationId)
    {
        string sql = $"SELECT COUNT(1) FROM {Table} WHERE ReservationId = @ReservationId";
        int count = _connection.ExecuteScalar<int>(sql, new { ReservationId = reservationId });
        return count > 0;
    }

    public void Delete(long reservationId)
    {
        string sql = $"DELETE FROM {Table} WHERE ReservationId = @ReservationId";
        _connection.Execute(sql, new { ReservationId = reservationId });
    }
}