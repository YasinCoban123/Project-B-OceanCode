namespace UnitTests
{
    [TestClass]
    public sealed class ScreeningAndDateTimeTests
    {
        [DataTestMethod]
        [DataRow("Interstellar", 4, 12, 1, "12-12-2026 21-00", "A sci-fi epic", "Matthew McConaughey, Anne Hathaway", "2:45")]
        [DataRow("Deadpool & Wolverine",2, 16, 2, "31-10-2026 19-00", "Superhero action movie", "Ryan Reynolds, Hugh Jackman", "3:00")]
        public void AddScreening_ValidData_ScreeningIsStored(
            string title,
            long genre,
            long pg,
            int hallNumber,
            string dateTime,
            string description,
            string actors,
            string duration)
        {
            ScreeningLogic screeningLogic = new();
            MovieLogic movieLogic = new();
            HallLogic hallLogic = new();

            movieLogic.CreateMovie(title, genre, pg, description, actors, duration);

            while (hallLogic.GetAllHalls().Count < hallNumber)
            {
                hallLogic.CreateAHall();
            }

            MovieModel movie = movieLogic.GetAllMovies().First(x => x.Title == title);
            HallModel hall = hallLogic.GetAllHalls().First(x => x.HallId == hallNumber);

            screeningLogic.AddScreening(movie.MovieId, hall.HallId, dateTime);

            var screenings = screeningLogic.GetAll();

            Assert.IsTrue(screenings.Any(s =>
                s.MovieId == movie.MovieId &&
                s.HallId == hall.HallId &&
                s.ScreeningStartingTime == dateTime));
        }


        [DataTestMethod]
        [DataRow("12/10/2014")]
        [DataRow("03-08-200")]
        public void CheckDate_InvalidDates_ReturnFalse(string date)
        {
            HallLogic hallLogic = new();

            bool result = hallLogic.CheckDate(date);

            Assert.IsFalse(result);
        }
        [DataTestMethod]
        [DataRow("21;00")]
        [DataRow("21-00")]
        public void CheckTime_InvalidTimes_ReturnFalse(string time)
        {
            HallLogic hallLogic = new();

            bool result = hallLogic.CheckTime(time);

            Assert.IsFalse(result);
        }
    }
}
