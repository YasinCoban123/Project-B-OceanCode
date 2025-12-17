using System;
using System.Collections.Generic;
using System.Linq;

static class ScreeningsAdmin
{
    static private ScreeningLogic screeningLogic = new ScreeningLogic();
    static private MovieLogic movielogic = new();
    static private HallLogic hallLogic = new();

    static List<MovieModel> allmovies => movielogic.GetAllMovies();
    static List<HallModel> allhalls => hallLogic.GetAllHalls();

    public static void Start()
    {
        Console.Clear();

        MenuHelper menu = new MenuHelper(new[]
        {
            "Show all screenings",
            "Add a screening",
            "Edit a screening",
            "Delete a screening",
            "Go Back"
        },
        "Screenings Admin");

        menu.Show();

        int choice = menu.SelectedIndex;

        Console.Clear();

        if (choice == 0)
        {
            ShowAllScreenings();
            PauseReturn();
        }
        else if (choice == 1)
        {
            AddScreening();
            PauseReturn();
        }
        else if (choice == 2)
        {
            EditScreening();
            PauseReturn();
        }
        else if (choice == 3)
        {
            DeleteScreening();
            PauseReturn();
        }
        else if (choice == 4)
        {
            Console.WriteLine("Returning...");
            //Console.ReadLine();
            Console.Clear();
            Menu.AdminStart();
        }
    }

    public static void AddScreening()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the add screening page");

        foreach (MovieModel movie in allmovies)
        {
            Console.WriteLine();
            Console.WriteLine($"Movie ID: {movie.MovieId}");
            Console.WriteLine($"Movie Title: {movie.Title}");
        }

        int chosenMovieID = 0;
        while (true)
        {
            Console.WriteLine("Choose Movie ID:");
            if (int.TryParse(Console.ReadLine(), out chosenMovieID) &&
                allmovies.Any(m => m.MovieId == chosenMovieID))
            {
                break;
            }
            Console.WriteLine("Invalid Movie ID.");
        }

        MovieModel ChosenMovie = allmovies.Find(Movie => chosenMovieID == Movie.MovieId);

        foreach (HallModel hall in allhalls)
        {
            Console.WriteLine($"Hall {hall.HallId}");
        }

        long chosenHallID = 0;
        while (true)
        {
            Console.WriteLine("Choose Hall ID:");
            if (long.TryParse(Console.ReadLine(), out chosenHallID) &&
                allhalls.Any(h => h.HallId == chosenHallID))
            {
                break;
            }
            Console.WriteLine("Invalid Hall ID.");
        }

        HallModel ChosenHall = allhalls.Find(Hall => chosenHallID == Hall.HallId);

        string Date = "";
        while (true)
        {
            Console.Write("Enter date (dd-MM-yyyy): ");
            Date = Console.ReadLine();
            if (hallLogic.CheckDate(Date)) break;
            Console.WriteLine("Invalid date.");
        }

        string Time = "";
        while (true)
        {
            Console.Write("Enter time (HH-mm): ");
            Time = Console.ReadLine();
            if (hallLogic.CheckTime(Time)) break;
            Console.WriteLine("Invalid time.");
        }

        if (screeningLogic.CheckScreeningOverlap((int)ChosenHall.HallId, ChosenMovie.MovieId, $"{Date} {Time}"))
        {
            Console.WriteLine("Screening overlaps with existing screening in the same hall. Cannot add screening.");
            return;
        }

        string DateTime = $"{Date} {Time}";
        screeningLogic.AddScreening(ChosenMovie.MovieId, ChosenHall.HallId, DateTime);

        Console.WriteLine("Screening added!");
    }

    public static void EditScreening()
    {
        Console.Clear();
        ShowAllScreenings();

        int chosenScreeningId = 0;
        while (true)
        {
            Console.WriteLine("Enter Screening ID to edit:");
            if (int.TryParse(Console.ReadLine(), out chosenScreeningId))
                break;

            Console.WriteLine("Invalid input.");
        }

        List<ScreeningModel> AllScreenings = screeningLogic.GetAll();
        ScreeningModel ChosenScreening = AllScreenings.Find(x => chosenScreeningId == x.ScreeningId);

        if (ChosenScreening == null)
        {
            Console.WriteLine("No screening found.");
            return;
        }

        int chosenMovieID = 0;
        while (true)
        {
            Console.WriteLine();
            foreach (MovieModel movie in allmovies)
            {
                Console.WriteLine();
                Console.WriteLine($"Movie ID: {movie.MovieId}");
                Console.WriteLine($"Movie Title: {movie.Title}");
            }
            Console.Write("Enter new Movie ID: ");
            if (int.TryParse(Console.ReadLine(), out chosenMovieID) &&
                allmovies.Any(m => m.MovieId == chosenMovieID))
            {
                break;
            }
            Console.WriteLine("Invalid Movie ID.");
        }

        ChosenScreening.MovieId = chosenMovieID;

        string Date = "";
        while (true)
        {
            Console.Write("Enter date (dd-MM-yyyy): ");
            Date = Console.ReadLine();
            if (hallLogic.CheckDate(Date)) break;
            Console.WriteLine("Invalid date.");
        }

        string Time = "";
        while (true)
        {
            Console.Write("Enter time (HH-mm): ");
            Time = Console.ReadLine();
            if (hallLogic.CheckTime(Time)) break;
            Console.WriteLine("Invalid time.");
        }

        string DateTime = $"{Date} {Time}";
        ChosenScreening.ScreeningStartingTime = DateTime;

        screeningLogic.Update(ChosenScreening);
        Console.WriteLine("Screening updated!");
    }

    public static void DeleteScreening()
    {
        Console.Clear();
        ShowAllScreenings();

        long id = 0;
        while (true)
        {
            Console.WriteLine("Enter Screening ID to delete:");
            if (long.TryParse(Console.ReadLine(), out id))
                break;

            Console.WriteLine("Invalid ID.");
        }

        screeningLogic.DeleteScreening(id);
        Console.WriteLine("Screening removed.");
    }

    public static void ShowAllScreenings()
    {
        List<string> screenings = screeningLogic.ShowScreenings();
        foreach (var s in screenings)
        {
            Console.WriteLine(s);
        }
    }

    private static void PauseReturn()
    {
        Console.WriteLine("\nPress ENTER to return...");
        Console.ReadLine();
        Console.Clear();
        Menu.AdminStart();
    }
}
