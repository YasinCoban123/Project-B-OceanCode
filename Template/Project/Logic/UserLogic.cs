using System;
using System.Collections.Generic;

public class UserLogic
{
    private ReservationAcces _reservationAccess = new();

    public IEnumerable<ReservationModel> GetReservationsForUser(long accountId)
    {
        return _reservationAccess.GetByAccountId(accountId);
    }

    public bool DeleteReservationIfExists(long reservationId)
    {
        if (_reservationAccess.Exists(reservationId))
        {
            _reservationAccess.Delete(reservationId);
            return true;
        }
        return false;
    }
}
