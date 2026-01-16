using System;
using System.Collections.Generic;

public class UserLogic
{
    private ReservationAcces _reservationAccess = new();

    public List<ReservationModel> GetReservationsForUser(long accountId)
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

    public string GetDateWithMostReservationsBetween(DateTime from, DateTime to)
    {
        return _reservationAccess.GetDateWithMostReservationsBetween(from, to);
    }

    public bool FilterBetweenCheck(string date1, string date2)
    {
        if (date1.Length != 10 || date1[2] != '-' || date1[5] != '-')
        {
            return false;
        }

        if (date2.Length != 10 || date2[2] != '-' || date2[5] != '-')
        {
            return false;
        }

        if (!DateTime.TryParse(date1, out DateTime d1))
        {
            return false;
        }

        if (!DateTime.TryParse(date2, out DateTime d2))
        {
            return false;
        }

        if (d1.Date > DateTime.Today || d2.Date > DateTime.Today)
        {
            return false;
        }

        if (d2 < d1)
        {
            return false;
        }

        return true;
    }
}