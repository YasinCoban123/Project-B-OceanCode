public class ScreeningLogic
{
    private ScreeningAcces _screeningAccess = new();
    private ReservationAcces _reservationAccess = new();
    private SeatAcces _seatAccess = new();
    private ReservedSeatAccess _reservedSeatAccess = new();



    public List<string> ShowScreenings()
    {
        return _screeningAccess.GetScreenings();
    }

    public bool MakeReservation(long accountId, int screeningId)
    {

        // haal screening op
        ScreeningModel screening = _screeningAccess.GetById(screeningId);
        if (screening == null)
        {
            return false;
        }

        // zoek vrije stoel
        SeatModel seat = _seatAccess.GetFirstEmptySeat(screening.HallId);
        if (seat == null)
        {
            return false;
        }

        // maak nieuwe reservation
        ReservationModel reservation = new ReservationModel
        {
            AccountId = accountId,
            ScreeningId = screeningId,
            ReservationTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        long reservationId = _reservationAccess.AddReservation(reservation);

        // koppel stoel aan reservering
        _seatAccess.MarkSeatAsReserved(seat.SeatId);
        _reservedSeatAccess.AddReservedSeat(reservationId, seat.SeatId);

        return true;
    }
}