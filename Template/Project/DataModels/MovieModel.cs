public class MovieModel
{

    public long MovieId { get; set; }

    public string Title { get; set; }

    public string Genre { get; set; }

    public long PGRating { get; set; }

    public MovieModel(long movieid, string title, string genre, long pgrating)
    {
        MovieId = movieid;
        Title = title;
        Genre = genre;
        PGRating = pgrating;
    }
}