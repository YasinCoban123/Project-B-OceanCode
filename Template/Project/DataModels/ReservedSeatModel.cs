public class ReservedSeatModel
{
    public long ReservedSeatId { get; set; }
    public long ReservationId { get; set; }
    public long SeatId { get; set; }

    public ReservedSeatModel(long reservedseatid, long reservationid, long seatid)
    {
        ReservedSeatId = reservedseatid;
        ReservationId = reservationid;
        SeatId = seatid;
    }

}