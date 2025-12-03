public class SeatModel
{
    public long SeatId { get; set; }
    public long HallId { get; set; }
    public long SeatTypeId { get; set; }
    public long RowNumber { get; set; }
    public long SeatNumber { get; set; }


    public SeatModel()
    {

    }
    public SeatModel(long seatId, long hallId, long seattypeId, long rownumber, long seatnumber)
    {
        SeatId = seatId;
        HallId = hallId;
        SeatTypeId = seattypeId;
        RowNumber = rownumber;
        SeatNumber = seatnumber;
    }
}