public class MovieLogic
{
    private MovieAcces _movieacces = new();


    public List<MovieModel> GetAllMovies()
    {
        return _movieacces.GetAllMovie();
    }

    public void CreateMovie(string title, string genre, long pgrating)
    {
        MovieModel movie = new(title, genre, pgrating);
        _movieacces.Write(movie);
    }

    public void DeleteMovie(MovieModel movie)
    {
        _movieacces.Delete(movie);
    }
}