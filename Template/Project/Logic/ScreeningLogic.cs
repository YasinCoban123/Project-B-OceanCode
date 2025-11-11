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

    public List<(long SeatId, int RowNumber, int SeatNumber, string TypeName, decimal Price, bool IsTaken)> GetSeatStatus(int screeningId)
    {
        var screening = _screeningAccess.GetById(screeningId);
        if (screening == null)
        {
            return new List<(long, int, int, string, decimal, bool)>();
        }

        return _seatAccess.GetSeatStatusByScreening(screening.HallId, screeningId);
    }

    public (bool Valid, List<int> FailedSeats) ValidateSeatsBeforeReservation(UserAccountModel user, int screeningId, List<int> seatIds)
    {
        if (seatIds == null || seatIds.Count == 0)
            return (false, seatIds);

        if (seatIds.Count > 20)
            return (false, seatIds);

        var screening = _screeningAccess.GetById(screeningId);
        if (screening == null)
            return (false, seatIds);

        int totalReservedForUser = _reservationAccess.GetSeatCountForUserScreening(user.AccountId, screeningId);
        if (totalReservedForUser + seatIds.Count > 20)
            return (false, seatIds);

        List<int> failedSeats = new();

        foreach (int seatId in seatIds)
        {
            var seat = _seatAccess.GetSeatById(seatId);
            if (seat == null || _reservedSeatAccess.IsSeatReserved(seatId, screeningId))
            {
                failedSeats.Add(seatId);
            }
        }

        bool valid = failedSeats.Count < seatIds.Count;
        return (valid, failedSeats);
    }

    public void ConfirmReservation(UserAccountModel user, int screeningId, List<int> seatIds)
    {
        foreach (int seatId in seatIds)
        {
            var seat = _seatAccess.GetSeatById(seatId);
            if (seat == null || _reservedSeatAccess.IsSeatReserved(seatId, screeningId))
                continue;

            var reservation = new ReservationModel(user.AccountId, screeningId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            long reservationId = _reservationAccess.AddReservation(reservation);
            _reservedSeatAccess.AddReservedSeat(reservationId, seatId, screeningId);
        }
    }

    public List<ReservationModel> GetAllReservations()
    {
        return _reservationAccess.GetAllReservations();
    }
}
