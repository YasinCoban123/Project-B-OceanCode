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
    static List<ScreeningModel> allscreenings => screeningLogic.GetAll();

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
            EditAScreening();
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

        MovieModel chosenmovie = SelectMovieArrow();


        HallModel chosenHall = SelectHallArrow();

        Console.Clear();

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

        if (screeningLogic.CheckScreeningOverlap((int)chosenHall.HallId, chosenmovie.MovieId, $"{Date} {Time}"))
        {
            Console.WriteLine("Screening overlaps with existing screening in the same hall. Cannot add screening.");
            return;
        }

        string DateTime = $"{Date} {Time}";
        screeningLogic.AddScreening(chosenmovie.MovieId, chosenHall.HallId, DateTime);

        Console.WriteLine("Screening added!");
    }

    public static void EditAScreening()
    {
        var table = new TableUI<ScreeningModel>
        (
            "All screenings (Select any screening to edit / go back)",
            new(
                [
                    new("ScreeningId", "Screening ID"),
                    new("MovieId", "Movie ID"),
                    new("HallId", "Hall ID"),
                    new("ScreeningStartingTime", "Start Time")
                ]
            ),
            allscreenings,
            ["MovieId", "HallId"]
        );

        ScreeningModel? chosen = table.Start();

        if (chosen is null)
        {
            return;
        }

        int ChosenID = Convert.ToInt32(chosen.ScreeningId);
        ScreeningModel chosenscreening = allscreenings.Find(x => x.ScreeningId == ChosenID);
        ScreeningModel screening = UpdateScreening(chosenscreening);
        screeningLogic.Update(screening);
    }
    public static ScreeningModel UpdateScreening(ScreeningModel screening)
        {
            var screeningEditor = new ItemEditor<ScreeningModel>(
                screening,
                "Edit screening screen",
                new List<EditOption<ScreeningModel>>
                {
                    new EditOption<ScreeningModel>
                    {
                        Label = "Movie",
                        Display = m => m.MovieId.ToString(),
                        OnSelect = m =>
                        {
                            MovieModel selectedMovie = SelectMovieArrow();
                            if (selectedMovie != null)
                            {
                                m.MovieId = selectedMovie.MovieId;
                            }
                        }
                    },
                    new EditOption<ScreeningModel>
                    {
                        Label = "Hall",
                        Display = m => m.HallId.ToString(),
                        OnSelect = m =>
                        {
                            HallModel selectedHall = SelectHallArrow();
                            if (selectedHall != null)
                            {
                                m.HallId = selectedHall.HallId;
                            }
                        }
                    },
                    new EditOption<ScreeningModel>
                    {
                        Label = "Starting Time",
                        Display = s => s.ScreeningStartingTime,
                        OnSelect = s =>
                        {
                            string date;
                            while (true)
                            {
                                Console.Write("Enter date (dd-MM-yyyy, leave blank to keep current): ");
                                date = Console.ReadLine();

                                if (string.IsNullOrWhiteSpace(date))
                                    return;

                                if (hallLogic.CheckDate(date))
                                    break;

                                Console.WriteLine("Invalid date.");
                            }

                            string time;
                            while (true)
                            {
                                Console.Write("Enter time (HH-mm): ");
                                time = Console.ReadLine();

                                if (hallLogic.CheckTime(time))
                                    break;

                                Console.WriteLine("Invalid time.");
                            }

                            s.ScreeningStartingTime = $"{date} {time}";
                        }
                    }
                }
            );

            return screeningEditor.Start();
        }

    public static void DeleteScreening()
    {
        Console.WriteLine();
        var table = new TableUI<ScreeningModel>
        (
            "All screenings (Select any screening to delete)",
            new(
                [
                    new("ScreeningId", "Screening ID"),
                    new("MovieId", "Movie ID"),
                    new("HallId", "Hall ID"),
                    new("ScreeningStartingTime", "Start Time")
                ]
            ),
            allscreenings,
            ["MovieId", "HallId"]
        );
        ScreeningModel? chosen = table.Start();

        if (chosen is null)
        {
            return;
        }

        long KeuzeID = Convert.ToInt64(chosen.ScreeningId);
        Console.Clear();

        ScreeningModel screening = allscreenings.Find(screening => KeuzeID == screening.ScreeningId);
        screeningLogic.DeleteScreening(screening);
        Console.WriteLine("Screening successfully deleted");
        Console.WriteLine("Press ENTER to continue");
        Console.ReadLine();
        Console.Clear();
    }

    public static void ShowAllScreenings()
    {
        var table = new TableUI<ScreeningModel>
        (
            "All screenings",
            new(
                [
                    new("ScreeningId", "Screening ID"),
                    new("MovieId", "Movie ID"),
                    new("HallId", "Hall ID"),
                    new("ScreeningStartingTime", "Start Time")
                ]
            ),
            allscreenings,
            ["MovieId", "HallId"]
        );

        table.Start();

    }

    private static void PauseReturn()
    {
        Console.WriteLine("\nPress ENTER to return...");
        //Console.ReadLine();
        Console.Clear();
        Menu.AdminStart();
    }

    public static MovieModel SelectMovieArrow()
    {
        List<string> MoviesTitles = allmovies.Select(x => $"{x.MovieId} - {x.Title}").ToList();

        MenuHelper menu = new MenuHelper(MoviesTitles, "Select a Movie:");
        menu.Show();

        return allmovies[menu.SelectedIndex];
    }

    public static HallModel SelectHallArrow()
    {
        List<string> Halls = allhalls.Select(x => $" Hall {x.HallId}").ToList();

        MenuHelper menu = new MenuHelper(Halls, "Select a Hall:");
        menu.Show();

        return allhalls[menu.SelectedIndex];
    }
}
