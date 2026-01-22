public class InspectMovie
{
    static private MovieLogic movieLogic = new MovieLogic();
    static private ReviewLogic reviewLogic = new ReviewLogic();

    public static void Start()
    {
        Console.Clear();

            List<MovieModel> movies = movieLogic.GetAllMovies().ToList();

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
            foreach (MovieModel m in movies)
            {
                moviesList.Add((MovieModel)m);
            }

        while (true)
        {
                TableUI<MovieModel> table = new TableUI<MovieModel>(
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

            // show movie details
            Console.WriteLine($"-------------------------------------------");
            Console.WriteLine($"Title: {chosen.Title}");
            Console.WriteLine();
            GenreModel? genre = GenreLogic.GetGenreById((int)chosen.GenreId);
            string genreName = genre != null ? genre.Genre : chosen.GenreId.ToString();
            Console.WriteLine();
            Console.WriteLine($"Genre: {genreName}");
            Console.WriteLine();
            Console.WriteLine($"PG Rating: {chosen.PGRating}");
            Console.WriteLine();
            Console.WriteLine($"Description: {chosen.Description}");
            Console.WriteLine();
            Console.WriteLine($"Actors: {chosen.Actors}");
            Console.WriteLine();
            Console.WriteLine($"Duration: {chosen.Duration}");
            Console.WriteLine($"-------------------------------------------");

            // options: add review, show reviews, back

            string[] actions = new string[] { "Add review", "Show reviews", "Back" };

            while (true)
            {
                for (int i = 0; i < actions.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {actions[i]}");
                }

                Console.Write("Choose an option: ");
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out int selected) || selected < 1 || selected > actions.Length)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid selection. Try again.");
                    continue;
                }

                string selectedAction = actions[selected - 1];

                if (selectedAction == "Add review")
                {
                    Console.Clear();
                    ReviewMovie.Start(chosen);
                    Console.Clear();
                    break;
                }

                if (selectedAction == "Show reviews")
                {
                    Console.Clear();
                    List<ReviewModel> reviews = reviewLogic.GetReviewsForMovie((int)chosen.MovieId).ToList();
                    if (!reviews.Any())
                    {
                        Console.WriteLine("No reviews for this movie yet.");
                        Console.WriteLine("Press enter to return to movie details");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    }

                    Console.WriteLine($"Reviews for {chosen.Title}:");
                    Console.WriteLine();

                    foreach (ReviewModel r in reviews)
                    {
                        Console.WriteLine($"Title: {r.Title}");
                        Console.WriteLine($"Rating: {r.Rating}/5");
                        Console.WriteLine($"Comment: {r.Comment}");
                        Console.WriteLine("-------------------------------------------");
                    }

                    Console.WriteLine("End of reviews. Press enter to return to movie details");
                    Console.ReadLine();

                    Console.Clear();
                    break;
                }

                if (selectedAction == "Back")
                {
                    Console.Clear();
                    break;
                }
            }
            
        }
    }
}
