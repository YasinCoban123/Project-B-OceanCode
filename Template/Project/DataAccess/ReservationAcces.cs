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
}