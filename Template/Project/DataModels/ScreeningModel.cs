public class ScreeningModel
{
    public long ScreeningId { get; set; }
    public long MovieId { get; set; }
    public long HallId { get; set; }
    public DateTime ScreeningStartingTime { get; set; }

    // public MovieModel Movie { get; set; }
    // public HallModel Hall { get; set; }
}