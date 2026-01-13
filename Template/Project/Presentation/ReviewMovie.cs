public static class ReviewMovie
{
    public static void Start(MovieModel movie)
    {
        long movieId = 0;
        try
        {
            var t = movie?.GetType();
            var idProp = t?.GetProperty("MovieId") ?? t?.GetProperty("Id") ?? t?.GetProperty("ID");
            if (idProp != null)
            {
                var idVal = idProp.GetValue(movie);
                if (idVal != null) movieId = Convert.ToInt64(idVal);
            }
        }
        catch { movieId = 0; }

        var logic = new ReviewLogic();
        List<ReviewModel> reviews = logic.GetReviewsForMovie(movieId);

        Console.Clear();

        {
            int rating = 0;
            while (rating < 1 || rating > 5)
            {
                Console.Write("Enter rating (1-5) or press Enter to cancel: ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    return;
                if (!int.TryParse(input, out rating) || rating < 1 || rating > 5)
                {
                    Console.WriteLine("Please enter a number between 1 and 5.");
                }
            }

            Console.Write("Add a short title (optional): ");
            var title = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(title))
                    return;
            Console.Write("Add a short comment (optional, max 300 chars): ");
            var comment = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(comment))
                    return;
            if (comment.Length > 300) comment = comment.Substring(0, 300);

            var model = new ReviewModel
            {
                MovieId = movieId,
                Title = title,
                Rating = rating,
                Comment = comment
            };

            try
            {
                logic.AddReview(model);
                Console.WriteLine("Review saved.");
            }
            catch
            {
                Console.WriteLine("Failed to save review.");
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            reviews = logic.GetReviewsForMovie(movieId);
        }
    }
}