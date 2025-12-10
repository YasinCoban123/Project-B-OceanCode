public class MovieModel
{

    public long MovieId { get; set; }

    public string Title { get; set; }

    public long GenreId { get; set; }

    public long PGRating { get; set; }

    public MovieModel()
    {
        
    }
    public MovieModel(string title, long genreid, long pgrating)
    {
        Title = title;
        GenreId = genreid;
        PGRating = pgrating;
    }
}