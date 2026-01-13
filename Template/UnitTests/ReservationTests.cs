using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ReservationDeleteTests
{
    private UserLogic userLogic;

    [TestInitialize]
    public void Setup()
    {
        userLogic = new UserLogic();
        AccountsLogic.SetCurrentUser(1);
    }

    [TestMethod]
    public void DeleteReservation_ExistingReservation_ReturnsTrue()
    {
        // Arrange
        var reservation = new ReservationModel(1, 1, "12:00");

        userLogic.AddReservation(reservation);

        // Act
        bool deleted = userLogic.DeleteReservationIfExists(reservation.ReservationId);

        // Assert
        Assert.IsTrue(deleted);
    }

    [TestMethod]
    public void DeleteReservation_RemovesReservationFromSystem()
    {
        // Arrange
        var reservation = new ReservationModel(2, 2, "14:00");

        userLogic.AddReservation(reservation);

        // Act
        userLogic.DeleteReservationIfExists(reservation.ReservationId);
        ReservationAcces acces = new();
        var all = acces.GetAllReservations();
        var found = all.Find(x => x.ReservationId == reservation.ReservationId);
        bool existsAfterDelete = found != null;

        // Assert
        Assert.IsFalse(existsAfterDelete);
    }

    [TestMethod]
    public void DeleteReservation_InvalidReservationId_ReturnsFalse()
    {
        // Arrange
        long invalidReservationId = 999999;

        // Act
        bool deleted = userLogic.DeleteReservationIfExists(invalidReservationId);

        // Assert
        Assert.IsFalse(deleted);
    }

    [TestMethod]
    public void NonAdminUser_AttemptToDeleteReservation_ReturnsFalse()
    {
        // Arrange
        var reservation = new ReservationModel(3, 3, "16:00")
        {
            ReservationId = 1003,
            RowNumber = 1,
            SeatNumber = 1
        };

        userLogic.AddReservation(reservation);
        AccountsLogic.SetCurrentUser(2);

        // Act
        bool deleted = userLogic.DeleteReservationIfExists(reservation.ReservationId);

        // Assert
        Assert.IsFalse(deleted);

        AccountsLogic.SetCurrentUser(1);
        userLogic.DeleteReservationIfExists(reservation.ReservationId);
    }
}
