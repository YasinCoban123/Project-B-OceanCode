using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography.X509Certificates;

public class Movie
{
    static MovieLogic logic = new MovieLogic();
    static List<MovieModel> Allmovies => logic.GetAllMovies();

    static private GenreLogic genreLogic = new GenreLogic();
    public static void Start()
    {
        string choice = "";
        do
        {
            OptionsMenu menu = new OptionsMenu(new(["See all movies", "Add movies","Edit movies", "Delete movies", "Go back"]));
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
                EditAMovie();

            }

            if (menu.Selected == 3)
            {
                DeleteAMovie();
            }

            else if (menu.Selected == 4)
            {
                Console.Clear();
            }
        } while (choice != "4");
    }

    public static void ShowAllMovies()
    {
        var table = new TableUI<MovieModel>(
            "All movies", 
            new(
                [
                    new("MovieId", "Movie ID"),
                    new("Title", "Title"),
                    new("GenreId", "Genre ID"),
                    new("PGRating", "PG Rating"),
                    new("Description", "Description"),
                    new("Actors", "Actors"),
                    new("Duration", "Duration")
                ]
            ),
            Allmovies,
            ["Title", "GenreId"]
        );

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
        Console.Write("Give the title of the movie: ");
        string movieTitle = Console.ReadLine();

        Console.Write("What is the genre ID of the movie: ");
        long movieGenreId = Convert.ToInt64(Console.ReadLine());

        Console.Write("What is the PG rating of the movie: ");
        long moviePGRating = Convert.ToInt64(Console.ReadLine());

        Console.Write("Give a short description of the movie: ");
        string description = Console.ReadLine();

        Console.Write("List the actors (comma separated): ");
        string actors = Console.ReadLine();

        Console.Write("What is the duration of the movie (e.g. 2h 15m): ");
        string duration = Console.ReadLine();

        logic.CreateMovie(
            movieTitle,
            movieGenreId,
            moviePGRating,
            description,
            actors,
            duration
        );

        Console.WriteLine("\nMovie successfully created!");
        Console.WriteLine("Press ENTER to continue");
        Console.ReadLine();
        Console.Clear();

            
    }

    public static void EditAMovie()
    {
        var table = new TableUI<MovieModel>
        (
            "All movies (Select a movie to edit)", 
            new(
                [
                    new("MovieId", "Movie ID"),
                    new("Title", "Title"),
                    new("GenreId", "Genre ID"),
                    new("PGRating", "PG Rating"),
                    new("Description", "Description"),
                    new("Actors", "Actors"),
                    new("Duration", "Duration")
                ]
            ),
            Allmovies,
            ["Title", "GenreId"]
        );

        table.Start();
        MovieModel chosen = table.Start();

        if (chosen is null)
        {
            return;
        }
        int ChosenID = Convert.ToInt32(chosen.MovieId);
        MovieModel chosenmovie = Allmovies.Find(x => x.MovieId == ChosenID);
        MovieModel movie = UpdateMovie(chosenmovie);
        logic.Update(movie);
    }

    public static MovieModel UpdateMovie(MovieModel movie)
    {
        var movieEditor = new ItemEditor<MovieModel>(
            movie,
            "Edit movie screen",
            new List<EditOption<MovieModel>>
            {
                new EditOption<MovieModel>
                {
                    Label = "Title",
                    Display = m => m.Title,
                    OnSelect = m =>
                    {
                        Console.Write("Enter new title (leave blank to keep current): ");
                        string newTitle = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newTitle))
                            m.Title = newTitle;
                    }
                },
                new EditOption<MovieModel>
                {
                    Label = "Genre",
                    Display = m => m.GenreId.ToString(),
                    OnSelect = m =>
                    {
                        GenreModel selectedGenre = SelectGenreArrow();
                        if (selectedGenre != null)
                        {
                            m.GenreId = selectedGenre.GenreId;
                        }
                    }
                },
                new EditOption<MovieModel>
                {
                    Label = "PG Rating",
                    Display = m => m.PGRating.ToString(),
                    OnSelect = m =>
                    {
                        Console.Write("Enter new PG rating (leave blank to keep current): ");
                        string input = Console.ReadLine();
                        if (long.TryParse(input, out long newRating))
                            m.PGRating = newRating;
                    }
                },
                new EditOption<MovieModel>
                {
                    Label = "Description",
                    Display = m => m.Description,
                    OnSelect = m =>
                    {
                        Console.Write("Enter new description (leave blank to keep current): ");
                        string newDesc = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newDesc))
                            m.Description = newDesc;
                    }
                },
                new EditOption<MovieModel>
                {
                    Label = "Actors",
                    Display = m => m.Actors,
                    OnSelect = m =>
                    {
                        Console.Write("Enter new actors (leave blank to keep current): ");
                        string newActors = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newActors))
                            m.Actors = newActors;
                    }
                },
                new EditOption<MovieModel>
                {
                    Label = "Duration",
                    Display = m => m.Duration,
                    OnSelect = m =>
                    {
                        Console.Write("Enter new duration (leave blank to keep current): ");
                        string newDuration = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newDuration))
                            m.Duration = newDuration;
                    }
                }
            }
        );

        return movieEditor.Start();
    }

                


        

     public static void DeleteAMovie()
    {
        var table = new TableUI<MovieModel>
        (
            "All movies (Select any movie to delete)", 
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

    public static GenreModel SelectGenreArrow()
    {
        List<GenreModel> genres = genreLogic.GetAllGenresObject();
        if (genres == null || genres.Count == 0)
            return null;

        List<string> genreNames = genres.Select(g => $"{g.GenreId} - {g.Genre}").ToList();

        MenuHelper menu = new MenuHelper(genreNames, "Select a genre:");
        menu.Show();

        return genres[menu.SelectedIndex];
    }




   
}