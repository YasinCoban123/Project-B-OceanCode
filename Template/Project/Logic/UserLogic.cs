using System;
using System.Collections.Generic;

public class UserLogic
{
    private ReservationAcces _reservationAccess = new();

    public IEnumerable<dynamic> GetReservationsForUser(long accountId)
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

    public void AddReservation(ReservationModel reservation)
    {
        _reservationAccess.AddReservation(reservation);
    }

    public string GetDateWithMostReservations()
    {
        return _reservationAccess.GetDateWithMostReservations();
    }

    public static UserAccountModel GetById(int id)
    {
        UserAccountsAccess userAccess = new();
        return userAccess.GetAccountByID(id);
    }
}
