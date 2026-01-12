using System.Text.Json;

public static class ReviewMovie
{
    private class ReviewRecord
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = "";
        public int Rating { get; set; }
        public string Comment { get; set; } = "";
    }

    public static void Start(MovieModel movie)
    {
        int rating = 0;
        while (rating < 1 || rating > 5)
        {
            Console.Clear();
            Console.WriteLine("Review: " + (movie?.Title ?? "<unknown>"));
            Console.Write("Enter rating (1-5): ");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out rating) || rating < 1 || rating > 5)
            {
                Console.WriteLine("Please enter a number between 1 and 5. Press Enter to try again.");
                Console.ReadLine();
                rating = 0;
            }
        }

        Console.WriteLine("You rated: " + new string('★', rating) + new string('☆', 5 - rating));
        Console.Write("Add a short comment (optional, max 300 chars): ");
        var comment = Console.ReadLine() ?? "";
        if (comment.Length > 300) comment = comment.Substring(0, 300);

        try
        {
            var reviewsFile = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "reviews.json"));

            int movieId = 0;
            try
            {
                var idProp = movie?.GetType().GetProperty("Id");
                if (idProp != null)
                {
                    var idVal = idProp.GetValue(movie);
                    if (idVal != null) movieId = Convert.ToInt32(idVal);
                }
            }
            catch { movieId = 0; }

            var record = new ReviewRecord
            {
                MovieId = movieId,
                Title = movie?.Title ?? "",
                Rating = rating,
                Comment = comment
            };

            List<ReviewRecord> list = new();
            if (File.Exists(reviewsFile))
            {
                try
                {
                    var existing = File.ReadAllText(reviewsFile);
                    list = JsonSerializer.Deserialize<List<ReviewRecord>>(existing) ?? new List<ReviewRecord>();
                }
                catch
                {
                    list = new List<ReviewRecord>();
                }
            }

            list.Add(record);
            var opts = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(reviewsFile, JsonSerializer.Serialize(list, opts));

            Console.WriteLine("Review saved.");
        }
        catch
        {
            Console.WriteLine("Failed to save review.");
        }

        Console.WriteLine("Press Enter to return.");
        Console.ReadLine();
    }
}