public class SeatModel
{
    public long SeatId { get; set; }
    public long HallId { get; set; }
    public string? Row { get; set; }
    public long Number { get; set; }
    public bool IsReserved { get; set; }
}