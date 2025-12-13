public class Movie
{
    static MovieLogic logic = new MovieLogic();
    static List<MovieModel> Allmovies = logic.GetAllMovies();
    public static void Start()
    {
        
        Console.WriteLine();
        Console.WriteLine("[1] See all movies");
        Console.WriteLine("[2] Add movies");
        Console.WriteLine("[3] Delete movies");
        Console.WriteLine("[4] Go back");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            ShowAllMovies();
        }

        if (choice == "2")
        {
            CreateAMovie();
            
        }

        if (choice == "3")
        {
            DeleteAMovie();
        }

        else if (choice == "4")
        {
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
            Console.Clear();
            Menu.AdminStart();
        }
    }

    public static void ShowAllMovies()
    {
        foreach (MovieModel movie in Allmovies)
            {
                Console.WriteLine();
                Console.WriteLine($"MovieID: {movie.MovieId}");
                Console.WriteLine($"Title: {movie.Title}");
                Console.WriteLine($"Genre: {movie.GenreId}");
                Console.WriteLine($"PGrating: {movie.PGRating}");
            }
    }

    public static void CreateAMovie()
    {
        Console.WriteLine();
        Console.WriteLine("Give the Title of the Movie: ");
        string MovieTitle = Console.ReadLine();
        Console.WriteLine("What is the genre of the movie");
        int MovieGenre = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("What is the PG Rating for the movie: ");
        string MoviePGRating = Console.ReadLine();
        long MoviePGrating = Convert.ToInt64(MoviePGRating);

        logic.CreateMovie(MovieTitle, MovieGenre, MoviePGrating);
        Console.WriteLine("Movie successfully created");
        Console.WriteLine("Press ENTER to continue");
        Console.ReadLine();
        Console.Clear();
            
    }

     public static void DeleteAMovie()
    {
        Console.WriteLine();
        ShowAllMovies();
        Console.Write("Enter the ID of the movie you want to delete: ");
        string keuzeID = Console.ReadLine();
        long KeuzeID = Convert.ToInt64(keuzeID);

        MovieModel Deletedmovie = Allmovies.Find(movie => KeuzeID == movie.MovieId);
        logic.DeleteMovie(Deletedmovie);
        Console.WriteLine("Movie successfully deleted");
        Console.WriteLine("Press ENTER to continue");
        Console.ReadLine();
        Console.Clear();
    }   
}