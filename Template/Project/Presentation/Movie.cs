public class Movie
{
    static MovieLogic logic = new MovieLogic();
    static List<MovieModel> Allmovies = logic.GetAllMovies();
    public static void Start()
    {
        
      MenuHelper menu = new MenuHelper(new[]{
            "See all Movies",
            "Edit Movie",
            "Delete Movie",
            "Go Back"
        },
        "Movie Options");

        menu.Show();
        switch (menu.SelectedIndex)
        {
            case 0:
                ShowAllMovies();
                break;

           case 1:
                CreateAMovie();
                break;

            case 2:
                DeleteAMovie();
                break;

            case 4:
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
                Console.Clear();
                Menu.AdminStart();
                break;
        }

        
    }

    public static void ShowAllMovies()
    {
        foreach (MovieModel movie in Allmovies)
            {
                Console.WriteLine();
                Console.WriteLine($"MovieID: {movie.MovieId}");
                Console.WriteLine($"Title: {movie.Title}");
                Console.WriteLine($"Genre: {movie.Genre}");
                Console.WriteLine($"PGrating: {movie.PGRating}");
            }
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
            Console.Clear();
    }

    public static void CreateAMovie()
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