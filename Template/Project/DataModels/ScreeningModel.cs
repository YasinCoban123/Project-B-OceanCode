public class ScreeningModel
{
    public long ScreeningId { get; set; }
    public long MovieId { get; set; }
    public long HallId { get; set; }
    public DateTime ScreeningStartingTime { get; set; }

    public ScreeningModel(long screeningId, long movieId, long hallId, DateTime screeningStartingTime)
    {
        ScreeningId = screeningId;
        MovieId = movieId;
        HallId = hallId;
        ScreeningStartingTime = screeningStartingTime;
    }
}