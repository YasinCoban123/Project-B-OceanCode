public class ReservationModel
{
    public long ReservationId { get; set; }
    public long AccountId { get; set; }
    public long ScreeningId { get; set; }
    public string ReservationTime { get; set; }

    public ReservationModel()
    {

    }

    public ReservationModel(long accountid, long screeningid, string reservationtime)
    {
        AccountId = accountid;
        ScreeningId = screeningid;
        ReservationTime = reservationtime;
    }

}