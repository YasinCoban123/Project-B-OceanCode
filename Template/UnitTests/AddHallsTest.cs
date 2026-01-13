namespace UnitTests
{
    [TestClass]
    public sealed class HallLogicTests
    {
        [TestMethod]
        public void CreateHall_WithBlueprintHall1_Success()
        {
            HallLogic hallLogic = new();
            SeatLogic seatLogic = new();

            HallModel blueprintHall1 = hallLogic.CreateAHall();
            hallLogic.CreateAHall();

            HallModel newHall = hallLogic.CreateAHall();
            seatLogic.DuplicateSeatsByHall(blueprintHall1, newHall.HallId);

            Assert.IsNotNull(newHall);
            Assert.AreEqual(3, newHall.HallId);
        }

        [TestMethod]
        public void CreateHall_WithBlueprintHall2_Success()
        {
            HallLogic hallLogic = new();
            SeatLogic seatLogic = new();

            hallLogic.CreateAHall();
            HallModel blueprintHall2 = hallLogic.CreateAHall();

            HallModel newHall = hallLogic.CreateAHall();
            seatLogic.DuplicateSeatsByHall(blueprintHall2, newHall.HallId);

            Assert.IsNotNull(newHall);
            Assert.AreEqual(3, newHall.HallId);
        }

        [TestMethod]
        public void CreateHall_WithNonExistingBlueprintHall1_Fails()
        {
            HallLogic hallLogic = new();
            SeatLogic seatLogic = new();

            HallModel fakeBlueprint = new HallModel { HallId = 1 };
            HallModel newHall = hallLogic.CreateAHall();

            bool exceptionThrown = false;
            try
            {
                seatLogic.DuplicateSeatsByHall(fakeBlueprint, newHall.HallId);
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void CreateHall_WithNonExistingBlueprintHall2_Fails()
        {
            HallLogic hallLogic = new();
            SeatLogic seatLogic = new();

            hallLogic.CreateAHall();

            HallModel fakeBlueprint = new HallModel{ HallId = 2 };
            HallModel newHall = hallLogic.CreateAHall();

            bool exceptionThrown = false;
            try
            {
                seatLogic.DuplicateSeatsByHall(fakeBlueprint, newHall.HallId);
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
    }
}
