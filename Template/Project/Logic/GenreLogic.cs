public class GenreLogic
{
    private GenreAcces _genreAccess = new GenreAcces();

    public List<string> GetAllGenres()
    {
        return _genreAccess.GetAllGenres();
    }
}
