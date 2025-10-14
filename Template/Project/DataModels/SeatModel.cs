public class SeatModel
{
    public long SeatId { get; set; }
    public long HallId { get; set; }
    public bool IsReserved { get; set; }

    public SeatModel()
    {

    }
    public SeatModel(long seatId, long hallId, bool isReserved)
    {
        SeatId = seatId;
        HallId = hallId;
        IsReserved = isReserved;
    }
}