namespace UnitTests
{
    [TestClass]
    public sealed class MovieLogicTests
    {
        [DataTestMethod]
        [DataRow("Avatar", 3, 12, "Description", "Acotrs here", "2:45")]
        [DataRow("Inertstellar", 3, 12, "Description", "Acotrs here", "15:45")]
        [DataRow("Matrix", 3, 12, "Description", "Acotrs here", "15:45")]
        public void CreateMovie_ValidData_ReturnsTrue(string title, long genre, long pg, string description, string actors, string duration)
        {
            MovieLogic logic = new();

            bool result = logic.CreateMovie(title, genre, pg, description, actors, duration);

            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow("", "SciFi", 12, "Description", "Acotrs here", "")]
        [DataRow("   ", "Action", 16, "Description", "", "20-11-2026 15:45")]
        [DataRow(null, "Drama", 6, "", "Acotrs here", "20-11-2026 15:45")]
        public void CreateMovie_InvalidTitle_ReturnsFalse(string title, long genre, long pg, string desc, string actors, string date)
        {
            MovieLogic logic = new();

            bool result = logic.CreateMovie(title, genre, pg, desc, actors, date);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CreateMovie_DifferentTitles_ReturnTrue()
        {
            MovieLogic logic = new();

            logic.CreateMovie("Avatar", 3, 12, "Description", "Acotrs here", "2:45");
            bool result = logic.CreateMovie("Inception", 3, 12,  "Description", "Acotrs here", "2:45");

            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow("Avatar TLA")]
        [DataRow("Matrix 2")]
        [DataRow("Inception")]
        public void CreateMovie_DuplicateTitle_ReturnsFalse(string title)
        {
            MovieLogic logic = new();

            logic.CreateMovie(title, 3, 12, "Description", "Acotrs here", "2:45");
            bool result = logic.CreateMovie(title, 2, 12, "Description", "Acotrs here", "2:45");

            Assert.IsFalse(result);
        }

        [DataTestMethod]
        [DataRow("Star Wars Episode V", 6, 6,  "Description", "Acotrs here", "2:45")]
        [DataRow("Interstellar Force", 2, 16, "Description", "Acotrs here", "2:45")]
        [DataRow("Titanic", 4, 12, "Description", "Acotrs here", "2:45")]
        public void CreateMovie_Success_ReturnsTrue_MessageDisplayed(string title, long genre, long pg, string description, string actors, string duration)
        {
            MovieLogic logic = new();

            bool result = logic.CreateMovie(title, genre, pg, description, actors, duration);

            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow("", 1, 12, "Description", "Acotrs here", "2:45")]
        [DataRow(" ", 7, 16,  "Description", "Acotrs here", "2:45")]
        [DataRow(null, 4, 6,  "Description", "Acotrs here", "2:45")]
        public void CreateMovie_Fail_ReturnsFalse_MessageDisplayed(string title, long genre, long pg, string description, string actors, string duration)
        {
            MovieLogic logic = new();

            bool result = logic.CreateMovie(title, genre, pg, description, actors, duration);

            Assert.IsFalse(result);
        }
    }
}
