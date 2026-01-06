public class GenreLogic
{
    private GenreAcces _genreAcces = new GenreAcces();
    private ReservationAcces _reservationAcces = new ReservationAcces();

    public List<string> GetAllGenres()
    {
        return _genreAcces.GetAllGenres();
    }

    public List<GenreModel> GetAllGenresObject()
    {
        return _genreAcces.GetAllGenresObject();
    }

    public string GetMostPopularGenre()
    {
        return _genreAcces.GetMostPopularGenre();
    }
}
