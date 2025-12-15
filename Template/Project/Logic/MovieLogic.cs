public class MovieLogic
{
    private MovieAcces _movieacces = new();


    public List<MovieModel> GetAllMovies()
    {
        return _movieacces.GetAllMovie();
    }


    public bool CreateMovie(string title, long genre, long pgrating)
    {
        if (string.IsNullOrWhiteSpace(title)) return false;
        if (_movieacces.ExistsByTitle(title)) return false;

        MovieModel movie = new(title, genre, pgrating);
        _movieacces.Write(movie);
        return true;
    }


    public void DeleteMovie(MovieModel movie)
    {
        _movieacces.Delete(movie);
    }

    public DateTime GetMovieDuration(long movieId)
    {
        long durationInMinutes = _movieacces.GetMovieDuration(movieId);
        return DateTime.MinValue.AddMinutes(durationInMinutes);
    }

    public DateTime GetDurationInRightFormat(DateTime GetMovieDuration)
    {
        return new DateTime().AddMinutes(GetMovieDuration.Minute).AddHours(GetMovieDuration.Hour);
    }
}