public class Movie
{
    public static void Start()
    {
        MovieLogic logic = new();
        List<MovieModel> Allmovies = logic.GetAllMovies();
        Console.WriteLine();
        Console.WriteLine("[1] See all movies");
        Console.WriteLine("[2] Add movies");
        Console.WriteLine("[3] Delete movies");
        Console.WriteLine("[4] Go back");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            foreach (MovieModel movie in Allmovies)
            {
                Console.WriteLine();
                Console.WriteLine($"MovieID: {movie.MovieId}");
                Console.WriteLine($"Title: {movie.Title}");
                Console.WriteLine($"Genre: {movie.Genre}");
                Console.WriteLine($"PGrating: {movie.PGRating}");
            }
            Start();
        }

        if (choice == "2")
        {
            Console.WriteLine();
            Console.WriteLine("Give the Title of the Movie: ");
            string MovieTitle = Console.ReadLine();
            Console.WriteLine("What is the genre of the movie");
            string MovieGenre = Console.ReadLine();
            Console.WriteLine("What is the PG Rating for the movie: ");
            string MoviePGRating = Console.ReadLine();
            long MoviePGrating = Convert.ToInt64(MoviePGRating);

            logic.CreateMovie(MovieTitle, MovieGenre, MoviePGrating);
        }

        if (choice == "3")
        {
            Console.WriteLine();
            Console.Write("Enter the ID of the movie you want to delete: ");
            string keuzeID = Console.ReadLine();
            long KeuzeID = Convert.ToInt64(keuzeID);

            MovieModel Deletedmovie = Allmovies.Find(movie => KeuzeID == movie.MovieId);
            logic.DeleteMovie(Deletedmovie);
        }

        else if (choice == "4")
        {
            Menu.AdminStart();
        }
    }
}