using Microsoft.Data.Sqlite;
using Dapper;
using System.Collections.Generic;

public class SeatAcces
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private string Table = "Seat";

    public List<SeatRowLogic> GetSeatRowsByScreening(long hallId, int screeningId)
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

        var rows = new List<SeatRowLogic>();
        var result = _connection.Query<(long, int, int, string, decimal, bool)>(sql, new { HallId = hallId, ScreeningId = screeningId });

        int currentRow = -1;
        SeatRowLogic row = null;

        foreach (var seat in result)
        {
            if (seat.Item2 != currentRow)
            {
                currentRow = seat.Item2;
                row = new SeatRowLogic { RowNumber = currentRow };
                rows.Add(row);
            }

            row.Seats.Add((seat.Item1, seat.Item3, seat.Item4, seat.Item5, seat.Item6));
        }

        return rows;
    }

    public SeatModel? GetSeatById(long seatId)
    {
        string sql = $"SELECT * FROM {Table} WHERE SeatId = @SeatId";
        return _connection.QueryFirstOrDefault<SeatModel>(sql, new { SeatId = seatId });
    }

    public decimal GetSeatPrice(long seatId)
    {
        string sql = @"
        SELECT st.Price
        FROM Seat s
        JOIN SeatType st ON s.SeatTypeId = st.SeatTypeId
        WHERE s.SeatId = @SeatId";
        return _connection.ExecuteScalar<decimal>(sql, new { SeatId = seatId });
    }

    public List<SeatModel> GetAllSeatsByHallId(long hallid)
    {
        string sql = $"SELECT SeatTypeId, RowNumber, SeatNumber FROM {Table} WHERE HallId = @HallId";
        return _connection.Query<SeatModel>(sql, new { AccountId = hallid }).ToList();   
    }
    
    public void DuplicateSeatsByHall(HallModel hall, long newHallid)
    {
       string sql = @$"INSERT INTO {Table} (HallId, SeatTypeId, RowNumber, SeatNumber) 
       SELECT @NewHallid, SeatTypeId, RowNumber, SeatNumber 
       FROM {Table} 
       WHERE HallId = @HallId;
       ";
       _connection.Execute(sql, new { NewHallid = newHallid, HallId = hall.HallId });
    }

    public void Delete(HallModel Hall)
    {
        string sql = $"DELETE FROM {Table} WHERE HallId = @HallId";
        _connection.Execute(sql, Hall);
    }

}
