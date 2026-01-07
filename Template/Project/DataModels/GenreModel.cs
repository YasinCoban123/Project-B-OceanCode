public class GenreModel
{

    public long GenreId { get; set; }
    public string Genre { get; set; }

    public GenreModel() { }
    public GenreModel(long gerneid, string genre)
    {
        GenreId = gerneid;
        Genre = genre;
    }
}