public class GenreLogic
{
    private GenreAcces _genreAcces = new GenreAcces();
    private ReservationAcces _reservationAcces = new ReservationAcces();

    public List<string> GetAllGenres()
    {
        return _genreAcces.GetAllGenres();
    }

    public List<GenreModel> GetAllGenresModel()
    {
        return _genreAcces.GetAllGenresModel();
    }

    public static GenreModel? GetGenreById(int id)
    {
        GenreAcces genreAcces = new GenreAcces();
        List<GenreModel> genres = genreAcces.GetAllGenresModel();
        return genres.FirstOrDefault(x => x.GenreId == id);
    }



    public List<GenreModel> GetAllGenresObject()
    {
        return _genreAcces.GetAllGenresModel();
    }

    public string GetMostPopularGenre()
    {
        return _genreAcces.GetMostPopularGenre();
    }
}
