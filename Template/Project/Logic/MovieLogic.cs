public class MovieLogic
{
    private MovieAcces _movieacces = new();


    public List<MovieModel> GetAllMovies()
    {
        return _movieacces.GetAllMovie();
    }

    public string GetMostPopularMovie()
    {
        return _movieacces.GetMostPopularMovie();
    }

    public bool CreateMovie(string title,long genre,long pgrating,string description,string actors,string duration)
    {
        if (string.IsNullOrWhiteSpace(title)) 
        {
            return false;
        }
        if (_movieacces.ExistsByTitle(title)) 
        {
            return false;
        }
        MovieModel movie = new MovieModel(
            title,
            genre,
            pgrating,
            description,
            actors,
            duration
        );

    _movieacces.Write(movie);
    return true;
}



    public void DeleteMovie(MovieModel movie)
    {
        _movieacces.Delete(movie);
    }

    public void Update(MovieModel movie)
    {
        _movieacces.Update(movie);
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

    public static MovieModel? GetById(int id)
    {
        MovieAcces movieacces = new();
        return movieacces.GetAllMovie().FirstOrDefault(x => x.MovieId == id);
    }
}