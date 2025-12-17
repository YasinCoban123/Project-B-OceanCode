public class MovieModel
{
    public long MovieId { get; set; }
    public string Title { get; set; }
    public long GenreId { get; set; }
    public long PGRating { get; set; }
    public string Description { get; set; }
    public string Actors { get; set; }
    public string Duration { get; set; }

    public MovieModel()
    {
    }

    // Constructor zonder MovieId (voor INSERT)
    public MovieModel(string title, long genreId, long pgRating, string description, string actors, string duration)
    {
        Title = title;
        GenreId = genreId;
        PGRating = pgRating;
        Description = description;
        Actors = actors;
        Duration = duration;
    }
}
