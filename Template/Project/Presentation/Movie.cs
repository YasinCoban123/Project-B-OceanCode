using System.Runtime.Intrinsics.Arm;

public class Movie
{
    static MovieLogic logic = new MovieLogic();
    static List<MovieModel> Allmovies = logic.GetAllMovies();
    public static void Start()
    {
        string choice = "";
        do
        {
            OptionsMenu menu = new OptionsMenu(new(["See all movies", "Add movies", "Delete movies", "Go back"]));
            choice = menu.Selected.ToString();

            if (menu.Selected == 0)
            {
                ShowAllMovies();
            }

            if (menu.Selected == 1)
            {
                CreateAMovie();

            }

            if (menu.Selected == 2)
            {
                DeleteAMovie();
            }

            else if (menu.Selected == 3)
            {
                Console.Clear();
            }

            Allmovies = logic.GetAllMovies();
        } while (choice != "3");
    }

    public static void ShowAllMovies()
    {
        var table = new TableUI<MovieModel>(
            "All movies", 
            new(
                [new("MovieId", "Movie ID"),
                new("Title", "Title"),
                new("Genre", "Genre"),
                new("PGRating", "PG rating")
                ]),
                Allmovies,
                ["Title", "Genre"]);
        table.Start();
        // foreach (MovieModel movie in Allmovies)
        //     {
        //         Console.WriteLine();
        //         Console.WriteLine($"MovieID: {movie.MovieId}");
        //         Console.WriteLine($"Title: {movie.Title}");
        //         Console.WriteLine($"Genre: {movie.Genre}");
        //         Console.WriteLine($"PGrating: {movie.PGRating}");
        //     }
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
        Console.Write("Enter the ID of the movie you want to delete: ");
        var table = new TableUI<MovieModel>(
            "Enter the ID of the movie you want to delete:", 
            new(
                [new("MovieId", "Movie ID"),
                new("Title", "Title"),
                new("Genre", "Genre"),
                new("PGRating", "PG rating")
                ]),
                Allmovies,
                ["Title", "Genre"]);
        
        MovieModel? movie = table.Start();

        if (movie is null)
        {
            return;
        }

        long KeuzeID = Convert.ToInt64(movie.MovieId);
        Console.Clear();

        MovieModel Deletedmovie = Allmovies.Find(movie => KeuzeID == movie.MovieId);

        logic.DeleteMovie(Deletedmovie);
        Console.WriteLine("Movie successfully deleted");
        Console.WriteLine("Press ENTER to continue");
        Console.ReadLine();
        Console.Clear();
    }   
}