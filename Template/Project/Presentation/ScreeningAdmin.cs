static class ScreeningsAdmin
{
    static private ScreeningLogic screeningLogic = new ScreeningLogic();
    static private MovieLogic movielogic = new();
    static private HallLogic hallLogic = new();

    static List<MovieModel> allmovies => movielogic.GetAllMovies();
    static List<HallModel> allhalls => hallLogic.GetAllHalls();

    public static void Start()
    {
        Console.WriteLine();
        Console.WriteLine("[1] Show all screenings");
        Console.WriteLine("[2] Add a screening");
        Console.WriteLine("[3] Edit a screening");
        Console.WriteLine("[4] Delete a screening");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            ShowAllScreenings();
        }

        if (choice == "2")
        {
            AddScreening();
        }

        else if (choice == "3")
        {
            EditScreening();
        }

        else if (choice == "4")
        {
            DeleteScreening();
        }
        else
        {
            Console.WriteLine("Invalid choice");
        }
    }

    public static void AddScreening()
    {
        Console.WriteLine();
        Console.WriteLine("Welcome to the add screening page");
        foreach(MovieModel movie in allmovies)
        {
            Console.WriteLine();
            Console.WriteLine($"Movie ID: {movie.MovieId}");
            Console.WriteLine($"Movie Title: {movie.Title}");
        }
        Console.WriteLine("Choose above which movie ID you want to add to the screening");
        string chosenMovieid = Console.ReadLine();
        int chosenMovieID = Convert.ToInt32(chosenMovieid);
        MovieModel ChosenMovie = allmovies.Find(Movie => chosenMovieID == Movie.MovieId);
        long MovieID = ChosenMovie.MovieId;
        Console.WriteLine();
        foreach(HallModel Hall in allhalls)
        {
            Console.WriteLine($"Hall {Hall.HallId}\n");
        }
        Console.WriteLine("Which Hall do you want the screening to be in");
        string chosenHallid = Console.ReadLine();
        long ChosenHallID = Convert.ToInt64(chosenHallid);
        HallModel ChosenHall = allhalls.Find(Hall => ChosenHallID == Hall.HallId);
        long HallID = ChosenHall.HallId;
        string Date = "";
        string Time = "";
        bool Datebool = true;
        while(Datebool)
        {
            Console.WriteLine();
            Console.Write("Give a Date when the screening should be played: ");
            Date = Console.ReadLine();
            if (hallLogic.CheckDate(Date))
            {
                break;
            }
            Console.WriteLine("Invalid Date, use format(dd-MM-yyyy)");
        }

        bool Timebool = true;
        while(Timebool)
        {
            Console.WriteLine();
            Console.Write("Give a Time when the screening should be played: ");
            Time = Console.ReadLine();
            if (hallLogic.CheckTime(Time))
            {
                break;
            }
            Console.WriteLine("Invalid Time,  use format(HH-mm)");
        }
        string DateTime = $"{Date} {Time}";
        screeningLogic.AddScreening(MovieID, HallID, DateTime);
        Console.WriteLine();
        Console.WriteLine("Screening Sucessfully added!\nPress ENTER to continue");
        Console.ReadLine();
        Console.Clear();
    }

    public static void EditScreening()
    {

        Console.WriteLine();
        ShowAllScreenings();
        Console.WriteLine();
        Console.WriteLine("Choose the ID of the screening you want to edit");
        string choice = Console.ReadLine();
        int ChosenScreeningId = Convert.ToInt32(choice);

        List<ScreeningModel> AllScreenings = screeningLogic.GetAll();
        ScreeningModel ChosenScreening = AllScreenings.Find(x => ChosenScreeningId == x.ScreeningId);

        if (ChosenScreening is null)
        {
            Console.WriteLine("There is no screening with that ID");
            EditScreening();
        }



        bool MovieDoesExist = true;
        while (MovieDoesExist == true)
        {
            Console.WriteLine();
            foreach(MovieModel movie in allmovies)
            {
                Console.WriteLine();
                Console.WriteLine($"Movie ID: {movie.MovieId}");
                Console.WriteLine($"Movie Title: {movie.Title}");
            }
            Console.WriteLine("Choose above which movie ID you want to add to the screening");
            string chosenMovieid = Console.ReadLine();
            int chosenMovieID = Convert.ToInt32(chosenMovieid);
            MovieModel ChosenMovie = allmovies.Find(Movie => chosenMovieID == Movie.MovieId);

            if (ChosenMovie is null)
            {
                Console.WriteLine("There is no Movie with that ID");
                continue;
            }
            else
            {
                ChosenScreening.MovieId = ChosenMovie.MovieId;
                break;
            }

        }
        
        string Date = "";
        string Time = "";
        bool Datebool = true;
        while(Datebool)
        {
            Console.WriteLine();
            Console.Write("Give a Date when the screening should be played: ");
            Date = Console.ReadLine();
            if (hallLogic.CheckDate(Date))
            {
                break;
            }
            Console.WriteLine("Invalid Date, use format(dd-MM-yyyy)");
        }

        bool Timebool = true;
        while(Timebool)
        {
            Console.WriteLine();
            Console.Write("Give a Time when the screening should be played: ");
            Time = Console.ReadLine();
            if (hallLogic.CheckTime(Time))
            {
                break;
            }
            Console.WriteLine("Invalid Time,  use format(HH-mm)");
        }
        string DateTime = $"{Date} {Time}";
        ChosenScreening.ScreeningStartingTime = DateTime;

        screeningLogic.Update(ChosenScreening);
        Console.WriteLine("Screening Sucessfully updated!\nPress ENTER to continue");
        Console.ReadLine();
        Console.Clear();

    }

    public static void DeleteScreening()
    {
        Console.WriteLine();
        ShowAllScreenings();

        Console.WriteLine("Choose a screening to delete");
        string choice = Console.ReadLine();
        long choiceint = Convert.ToInt64(choice);
        screeningLogic.DeleteScreening(choiceint);
        Console.WriteLine("Screening Sucessfully deleted!\nPress ENTER to continue");
        Console.ReadLine();
        Console.Clear();

    }

    public static void ShowAllScreenings()
    {
        List<string> screenings = screeningLogic.ShowScreenings();
        foreach (var s in screenings)
        {
            Console.WriteLine(s);
        }
    }
}
