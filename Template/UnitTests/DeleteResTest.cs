namespace UnitTests
{
    [TestClass]
    public class DeleteResTests
    {
        private UserLogic userLogic;

        [TestInitialize]
        public void Setup()
        {
            userLogic = new UserLogic();
            AccountsLogic.SetCurrentUser(2);
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

            // Assert
            Assert.IsNull(found);
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
        public void DeleteFails_Within24Hours_ReturnsFalse()
        {
            // Arrange
            var screeningTime = System.DateTime.Now.AddHours(23).ToString();
            var reservation = new ReservationModel(3, 3, screeningTime);
            userLogic.AddReservation(reservation);

            // Act
            bool deleted = userLogic.DeleteReservationIfExists(reservation.ReservationId);

            // Assert
            Assert.IsFalse(deleted);

            AccountsLogic.SetCurrentUser(1);
            userLogic.DeleteReservationIfExists(reservation.ReservationId);
        }

        [TestMethod]
        public void RefundGiven_24HoursBeforeStart_DeletionReturnsTrue()
        {
            // Arrange
            var screeningTime = System.DateTime.Now.AddHours(48).ToString();
            var reservation = new ReservationModel(4, 4, screeningTime);
            userLogic.AddReservation(reservation);

            // Act
            bool deleted = userLogic.DeleteReservationIfExists(reservation.ReservationId);

            // Assert
            Assert.IsTrue(deleted);
        }

        [TestMethod]
        public void OptionForDeletingReservationsPopsUp_SystemTest()
        {
            Assert.Inconclusive("System test: verify UI shows option 'Delete' or 'Exit' under reservations list.");
        }

        [TestMethod]
        public void ConfirmationMessageDisplays_SystemTest()
        {
            Assert.Inconclusive("System test: verify confirmation prompt appears when user selects delete.");
        }

        [TestMethod]
        public void UserCancelsDeletion_SystemTest()
        {
            Assert.Inconclusive("System test: verify reservation remains when user cancels deletion confirmation.");
        }

        [TestMethod]
        public void SystemErrorWhenDeleting_SystemTest()
        {
            Assert.Inconclusive("System test: simulate backend failure during delete and verify user error notification.");
        }
    }
}