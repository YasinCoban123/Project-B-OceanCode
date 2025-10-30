public class ReservedSeatModel
{
    public long ReservedSeatId { get; set; }
    public long ReservationId { get; set; }
    public long ScreeningId { get; set; }
    public long SeatId { get; set; }
    public bool IsReserved { get; set; }


    public ReservedSeatModel(long reservedseatid, long reservationid, long screeningid, long seatid, bool isreserved)
    {
        ReservedSeatId = reservedseatid;
        ReservationId = reservationid;
        ScreeningId = screeningid;
        SeatId = seatid;
        IsReserved = isreserved;
    }

}