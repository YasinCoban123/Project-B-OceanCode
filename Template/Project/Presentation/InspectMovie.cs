public class InspectMovie
{
    static private MovieLogic movieLogic = new();
    static private ReviewLogic reviewLogic = new();

    public static void Start()
    {
        Console.Clear();

        var movies = movieLogic.GetAllMovies().ToList();

        if (!movies.Any())
        {
            Console.WriteLine("No movies available to review.");
            Console.WriteLine("Press enter to return to menu");
            Console.ReadLine();
            Console.Clear();
            Menu.Start();
            return;
        }

        List<MovieModel> moviesList = new List<MovieModel>();
        foreach (var m in movies)
        {
            moviesList.Add((MovieModel)m);
        }

        while (true)
        {
            var table = new TableUI<MovieModel>(
                "Available Movies", 
                new(
                    [new("Title", "Title")]
                    ),
                moviesList,
                ["Title", "GenreId", "PGRating"]);
            MovieModel? chosen = table.Start();

            if (chosen is null)
            {
                return;
            }

            Console.Clear();


            var fields = new Dictionary<string, string?>
            {
                ["Title"] = chosen.Title,
                ["Description"] = chosen.Description,
                ["Duration"] = chosen.Duration?.ToString(),
                ["PG Rating"] = chosen.PGRating.ToString(),
                ["Actors"] = chosen.Actors,
                ["Genre ID"] = chosen.GenreId.ToString() 
            };

            int consoleWidth = Math.Max(60, Console.WindowWidth);
            int labelWidth = fields.Keys.Max(k => k.Length) + 2;
            int valueMax = consoleWidth - labelWidth - 6;
            if (valueMax < 20) valueMax = 20;


            string[] WrapLines(string? text, int width)
            {
                if (string.IsNullOrEmpty(text)) return new[] { string.Empty };

                var words = text!.Split(' ');
                var lines = new List<string>();
                var current = "";

                foreach (var w in words)
                {
                    if (current.Length == 0)
                    {
                        current = w;
                    }
                    else if (current.Length + 1 + w.Length <= width)
                    {
                        current += " " + w;
                    }
                    else
                    {
                        lines.Add(current);
                        current = w;
                    }
                }

                if (current.Length > 0) lines.Add(current);
                return lines.ToArray();
            }

            string hr = new string('-', consoleWidth);
            Console.WriteLine(hr);
            foreach (var kv in fields)
            {
                var label = kv.Key.PadRight(labelWidth);
                if (kv.Value is null)
                {
                    Console.WriteLine($"{label} :");
                    continue;
                }

                var wrapped = WrapLines(kv.Value, valueMax);
                for (int i = 0; i < wrapped.Length; i++)
                {
                    if (i == 0)
                        Console.WriteLine($"{label} : {wrapped[i]}");
                    else
                        Console.WriteLine($"{new string(' ', labelWidth)}   {wrapped[i]}");
                }
                Console.WriteLine();
            }
            Console.WriteLine(hr);


            string[] actions = new[] { "Show reviews", "Add review", "Go back" };
            int selected = 0;
            int menuTop = Console.CursorTop;
            Console.WriteLine("Use arrow key up/down to choose and Enter to select.");
            menuTop = Console.CursorTop;
            void RenderMenu()
            {
                var origBg = Console.BackgroundColor;
                var origFg = Console.ForegroundColor;
                Console.SetCursorPosition(0, menuTop);
                for (int i = 0; i < actions.Length; i++)
                {
                    bool isSelected = i == selected;
                    if (isSelected)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = origBg;
                        Console.ForegroundColor = origFg;
                    }

                    var line = ("  " + actions[i]).PadRight(Console.WindowWidth);
                    Console.WriteLine(line);
                }
                Console.BackgroundColor = origBg;
                Console.ForegroundColor = origFg;
            }

            RenderMenu();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow)
                {
                    selected = (selected - 1 + actions.Length) % actions.Length;
                    RenderMenu();
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    selected = (selected + 1) % actions.Length;
                    RenderMenu();
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }

            if (actions[selected] == "Add review")
            {
                Console.Clear();
                ReviewMovie.Start(chosen);
                Console.Clear();
                continue;
            }

            if (actions[selected] == "Show reviews")
            {
                Console.Clear();
                var reviews = reviewLogic.GetReviewsForMovie((int)chosen.MovieId).ToList();
                if (!reviews.Any())
                {
                    Console.WriteLine("No reviews for this movie yet.");
                    Console.WriteLine("Press enter to return to movie details");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                Console.WriteLine($"Reviews for {chosen.Title}:");
                Console.WriteLine();

                foreach (var r in reviews)
                {
                    var stars = new string('★', Math.Max(0, Math.Min(5, r.Rating)));
                    var empties = new string('☆', 5 - Math.Max(0, Math.Min(5, r.Rating)));
                    Console.WriteLine($"[{stars}{empties}] {r.Title}");
                    if (!string.IsNullOrWhiteSpace(r.Comment)) Console.WriteLine("  " + r.Comment);
                    Console.WriteLine();
                }

                Console.WriteLine("End of reviews. Press enter to return to movie details");
                Console.ReadLine();
            
            Console.Clear();
        }
    }
}
}