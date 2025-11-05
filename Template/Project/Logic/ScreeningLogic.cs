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
        Console.WriteLine("Screening not found.");
        return new List<(long, int, int, string, decimal, bool)>();
    }
    
        return _seatAccess.GetSeatStatusByScreening(screening.HallId, screeningId);
    }


    public bool MakeReservation(long accountId, int screeningId, List<int> seatIds)
    {
        if (seatIds == null || seatIds.Count == 0)
        {
            Console.WriteLine("You must select at least one seat.");
            return false;
        }

        if (seatIds.Count > 20)
        {
            Console.WriteLine("You can reserve a maximum of 20 seats at once.");
            return false;
        }

        var screening = _screeningAccess.GetById(screeningId);
        if (screening == null)
        {
            Console.WriteLine("Screening not found.");
            return false;
        }

        int totalReservedForUser = _reservationAccess.GetSeatCountForUserScreening(accountId, screeningId);
        if (totalReservedForUser + seatIds.Count > 20)
        {
            Console.WriteLine($"You already have {totalReservedForUser} seats reserved. You can only reserve {20 - totalReservedForUser} more.");
            return false;
        }

        bool anySeatReserved = false;

        foreach (int seatId in seatIds)
        {
            if (_seatAccess.GetSeatById(seatId) == null)
            {
                Console.WriteLine($"SeatId {seatId} does not exist. Please enter a valid seat id.");
                continue;
            }

            if (_reservedSeatAccess.IsSeatReserved(seatId, screeningId))
            {
                Console.WriteLine($"Seat {seatId} could not be reserved (already taken).");
                continue;
            }

            var reservation = new ReservationModel(accountId, screeningId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            long reservationId = _reservationAccess.AddReservation(reservation);
            _reservedSeatAccess.AddReservedSeat(reservationId, seatId, screeningId);

            Console.WriteLine($"Seat {seatId} reserved successfully (Reservation ID: {reservationId}).");
            anySeatReserved = true;
        }


        if (!anySeatReserved)
        {
            Console.WriteLine("No seats could be reserved.");
            return false;
        }

        return true;
    }

    public List<ReservationModel> GetAllReservations()
    {
        return _reservationAccess.GetAllReservations();
    }

}