public class SeatRowLogic
{
    public int RowNumber { get; set; }
    public List<(long SeatId, int SeatNumber, string TypeName, decimal Price, bool IsTaken)> Seats { get; set; }

    public SeatRowLogic()
    {
        Seats = new List<(long, int, string, decimal, bool)>();
    }
}
