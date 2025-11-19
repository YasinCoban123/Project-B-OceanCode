public class ScreeningModel
{
    public long ScreeningId { get; set; }
    public long MovieId { get; set; }
    public long HallId { get; set; }
    public string ScreeningStartingTime { get; set; }

    public ScreeningModel(long screeningId, long movieId, long hallId, string screeningStartingTime)
    {
        ScreeningId = screeningId;
        MovieId = movieId;
        HallId = hallId;
        ScreeningStartingTime = screeningStartingTime;
    }

    public ScreeningModel(long movieId, long hallId, string screeningStartingTime)
    {
        MovieId = movieId;
        HallId = hallId;
        ScreeningStartingTime = screeningStartingTime;
    }
}