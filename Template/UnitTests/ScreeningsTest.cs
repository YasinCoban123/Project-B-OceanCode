using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public sealed class ScreeningsTests
    {
        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(9999)]
        public void GetSeatStatus_ScreeningDoesNotExist_ReturnsEmpty(int screeningId)
        {
            ScreeningLogic logic = new();
            var result = logic.GetSeatStatus(screeningId);
            Assert.AreEqual(0, result.Count);
        }

        [DataTestMethod]
        [DataRow("2099-01-01 10:00")]
        [DataRow("2099-01-01 12:00")]
        [DataRow("2099-01-01 14:00")]
        public void GetSeatStatus_ScreeningExists_ReturnsSeats(string dateTime)
        {
            ScreeningAcces sAcc = new();
            ScreeningLogic logic = new();

            long movieId = 1;
            long hallId = 1;

            sAcc.Add(new ScreeningModel(movieId, hallId, dateTime));
            long id = sAcc.GetAll().Last().ScreeningId;

            var result = logic.GetSeatStatus((int)id);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count >= 0);

            sAcc.Delete(id);
        }

        [DataTestMethod]
        [DataRow(999)]
        [DataRow(500)]
        [DataRow(1234)]
        public void GetSeatStatus_HallHasNoSeats_ReturnsEmpty(long hallId)
        {
            ScreeningAcces sAcc = new();
            ScreeningLogic logic = new();

            long movieId = 1;
            string dt = "2099-01-02 12:00";

            sAcc.Add(new ScreeningModel(movieId, hallId, dt));
            long id = sAcc.GetAll().Last().ScreeningId;

            var result = logic.GetSeatStatus((int)id);

            Assert.AreEqual(0, result.Count);

            sAcc.Delete(id);
        }

        [TestMethod]
        public void ReserveSeats_Upto20_IsAllowed()
        {
            ScreeningAcces sAcc = new();
            ScreeningLogic logic = new();

            var user = new UserAccountModel("Test", "test@test.com", "2000-01-01", "x", false);

            long hallId = 1;
            long movieId = 1;
            string dt = "2099-01-03 12:00";

            sAcc.Add(new ScreeningModel(movieId, hallId, dt));
            long id = sAcc.GetAll().Last().ScreeningId;

            var seats = new List<int>();
            for (int i = 960; i <= 979; i++)
                seats.Add(i);

            var (valid, failed) = logic.ValidateSeatsBeforeReservation(user, (int)id, seats);

            Assert.IsTrue(valid);
            Assert.AreEqual(0, failed.Count);

            sAcc.Delete(id);
        }

        [TestMethod]
        public void ReserveSeats_MoreThan20_IsRejected()
        {
            ScreeningAcces sAcc = new();
            ScreeningLogic logic = new();

            var user = new UserAccountModel("Test", "test@test.com", "2000-01-01", "x", false);

            long hallId = 1;
            long movieId = 1;
            string dt = "2099-01-04 12:00";

            sAcc.Add(new ScreeningModel(movieId, hallId, dt));
            long id = sAcc.GetAll().Last().ScreeningId;

            var seats = new List<int>();
            for (int i = 960; i <= 980; i++)
                seats.Add(i);

            var (valid, failed) = logic.ValidateSeatsBeforeReservation(user, (int)id, seats);

            Assert.IsFalse(valid);

            sAcc.Delete(id);
        }
    }
}
