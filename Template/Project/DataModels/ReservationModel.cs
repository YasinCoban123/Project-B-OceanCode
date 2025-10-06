public class Reservation
{
    public long ReservationId { get; set; }
    public long AccountId { get; set; }
    public string ReservationTime { get; set; }

    public Reservation(long reservationid, long accountid, string reservationtime)
    {
        ReservationId = reservationid;
        AccountId = accountid;
        ReservationTime = reservationtime;
    }

}