namespace UnitTests
{
    [TestClass]
    public sealed class MovieLogicTests
    {
        [DataTestMethod]
        [DataRow("Avatar", "SciFi", 12)]
        [DataRow("Inception", "Thriller", 16)]
        [DataRow("Matrix", "SciFi", 12)]
        public void CreateMovie_ValidData_ReturnsTrue(string title, string genre, long pg)
        {
            MovieLogic logic = new();

            bool result = logic.CreateMovie(title, genre, pg);

            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow("", "SciFi", 12)]
        [DataRow("   ", "Action", 16)]
        [DataRow(null, "Drama", 6)]
        public void CreateMovie_InvalidTitle_ReturnsFalse(string title, string genre, long pg)
        {
            MovieLogic logic = new();

            bool result = logic.CreateMovie(title, genre, pg);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CreateMovie_DifferentTitles_ReturnTrue()
        {
            MovieLogic logic = new();

            logic.CreateMovie("Avatar", "SciFi", 12);
            bool result = logic.CreateMovie("Inception", "SciFi", 12);

            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow("Avatar TLA")]
        [DataRow("Matrix 2")]
        [DataRow("Inception")]
        public void CreateMovie_DuplicateTitle_ReturnsFalse(string title)
        {
            MovieLogic logic = new();

            logic.CreateMovie(title, "SciFi", 12);
            bool result = logic.CreateMovie(title, "SciFi", 12);

            Assert.IsFalse(result);
        }

        [DataTestMethod]
        [DataRow("Star Wars Episode V", "SciFi", 6)]
        [DataRow("Interstellar Force", "SciFi", 16)]
        [DataRow("Titanic", "Drama", 12)]
        public void CreateMovie_Success_ReturnsTrue_MessageDisplayed(string title, string genre, long pg)
        {
            MovieLogic logic = new();

            bool result = logic.CreateMovie(title, genre, pg);

            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow("", "SciFi", 12)]
        [DataRow(" ", "Action", 16)]
        [DataRow(null, "Drama", 6)]
        public void CreateMovie_Fail_ReturnsFalse_MessageDisplayed(string title, string genre, long pg)
        {
            MovieLogic logic = new();

            bool result = logic.CreateMovie(title, genre, pg);

            Assert.IsFalse(result);
        }
    }
}
