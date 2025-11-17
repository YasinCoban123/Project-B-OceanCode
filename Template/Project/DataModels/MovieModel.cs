public class MovieModel
{

    public long MovieId { get; set; }

    public string Title { get; set; }

    public string Genre { get; set; }

    public long PGRating { get; set; }

    public MovieModel()
    {
        
    }
    public MovieModel(string title, string genre, long pgrating)
    {
        Title = title;
        Genre = genre;
        PGRating = pgrating;
    }
}