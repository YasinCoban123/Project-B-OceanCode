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
        var idMapper = new Dictionary<string, Func<MovieModel, string>>
        {
            { "GenreId", x => GenreLogic.GetGenreById((int)x.GenreId).Genre }
        };

        var table = new TableUI<MovieModel>(
            "All movies", 
            new(
                [
                    new("Title", "Title"),
                    new("GenreId", "Genre"),
                    new("PGRating", "PG Rating"),
                    new("Description", "Description"),
                    new("Actors", "Actors"),
                    new("Duration", "Duration")
                ]
            ),
            Allmovies,
            ["Title"],
            idMapper
        );

        table.Start();
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
        var idMapper = new Dictionary<string, Func<MovieModel, string>>
        {
            { "GenreId", x => GenreLogic.GetGenreById((int)x.GenreId).Genre }
        };

        var table = new TableUI<MovieModel>
        (
            "All movies (Select a movie to edit)", 
            new(
                [
                    new("Title", "Title"),
                    new("GenreId", "Genre"),
                    new("PGRating", "PG Rating"),
                    new("Description", "Description"),
                    new("Actors", "Actors"),
                    new("Duration", "Duration")
                ]
            ),
            Allmovies,
            ["Title"],
            idMapper
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
                    TryApply = (m, input) =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return (true, null);

                        m.Title = input;
                        return (true, null);
                    }
                },

                new EditOption<MovieModel>
                {
                    Label = "Genre",
                    Display = m => m.GenreId.ToString(),
                    TryApply = (m, input) =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return (true, null);

                        if (!long.TryParse(input, out long newGenreId))
                            return (false, "Genre ID must be a number.");

                        m.GenreId = newGenreId;
                        return (true, null);
                    }
                },

                new EditOption<MovieModel>
                {
                    Label = "PG Rating",
                    Display = m => m.PGRating.ToString(),
                    TryApply = (m, input) =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return (true, null);

                        if (!long.TryParse(input, out long newRating))
                            return (false, "PG rating must be a number.");

                        m.PGRating = newRating;
                        return (true, null);
                    }
                },

                new EditOption<MovieModel>
                {
                    Label = "Description",
                    Display = m => m.Description,
                    TryApply = (m, input) =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return (true, null);

                        m.Description = input;
                        return (true, null);
                    }
                },

                new EditOption<MovieModel>
                {
                    Label = "Actors",
                    Display = m => m.Actors,
                    TryApply = (m, input) =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return (true, null);

                        m.Actors = input;
                        return (true, null);
                    }
                },

                new EditOption<MovieModel>
                {
                    Label = "Duration",
                    Display = m => m.Duration,
                    TryApply = (m, input) =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return (true, null);

                        m.Duration = input;
                        return (true, null);
                    }
                }
            }
        );

        return movieEditor.Start();
    }

     public static void DeleteAMovie()
    {
        var idMapper = new Dictionary<string, Func<MovieModel, string>>
        {
            { "GenreId", x => GenreLogic.GetGenreById((int)x.GenreId).Genre }
        };

        var table = new TableUI<MovieModel>
        (
            "All movies (Select any movie to delete)", 
            new(
                [
                new("Title", "Title"),
                new("GenreId", "Genre"),
                new("PGRating", "PG rating")
                ]),
                Allmovies,
                ["Title"],
                idMapper
            );
        
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