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
}