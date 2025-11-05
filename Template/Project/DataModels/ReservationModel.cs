public class ReservationModel
{
    public long ReservationId { get; set; }
    public long AccountId { get; set; }
    public long ScreeningId { get; set; }
    public string ReservationTime { get; set; }

    public string? MovieTitle { get; set; }
    public string? ScreeningStartingTime { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }

    public ReservationModel(long accountid, long screeningid, string reservationtime)
    {
        AccountId = accountid;
        ScreeningId = screeningid;
        ReservationTime = reservationtime;
    }

}