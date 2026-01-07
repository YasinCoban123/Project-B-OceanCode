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
        var idMapper = new Dictionary<string, Func<ScreeningModel, string>>
        {
            { "MovieId", x => MovieLogic.GetById((int)x.MovieId).Title }
        };

        var table = new TableUI<ScreeningModel>
        (
            "All screenings (Select any screening to edit / go back)",
            new(
                [
                    new("ScreeningId", "Screening ID"),
                    new("MovieId", "Movie"),
                    new("HallId", "Hall ID"),
                    new("ScreeningStartingTime", "Start Time")
                ]
            ),
            allscreenings,
            ["HallId"],
            idMapper
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
                        Label = "Movie ID",
                        Display = s => s.MovieId.ToString(),
                        TryApply = (s, input) =>
                        {
                            if (string.IsNullOrWhiteSpace(input))
                                return (true, null);

                            if (!long.TryParse(input, out long newMovieId))
                                return (false, "Movie ID must be a number.");

                            if (!allmovies.Any(m => m.MovieId == newMovieId))
                                return (false, "Movie ID does not exist.");

                            s.MovieId = newMovieId;
                            return (true, null);
                        }
                    },
                    new EditOption<ScreeningModel>
                    {
                        Label = "Hall ID",
                        Display = s => s.HallId.ToString(),
                        TryApply = (s, input) =>
                        {
                            if (string.IsNullOrWhiteSpace(input))
                                return (true, null);

                            if (!long.TryParse(input, out long newHallId))
                                return (false, "Hall ID must be a number.");

                            if (!allhalls.Any(h => h.HallId == newHallId))
                                return (false, "Hall ID does not exist.");

                            s.HallId = newHallId;
                            return (true, null);
                        }
                    },
                    new EditOption<ScreeningModel>
                    {
                        Label = "Starting Time",
                        Display = s => s.ScreeningStartingTime,
                        TryApply = (s, input) =>
                        {
                            if (string.IsNullOrWhiteSpace(input))
                                return (true, null);

                            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length != 2)
                                return (false, "Format must be: dd-MM-yyyy HH-mm");

                            string date = parts[0];
                            string time = parts[1];

                            if (!hallLogic.CheckDate(date))
                                return (false, "Invalid date.");

                            if (!hallLogic.CheckTime(time))
                                return (false, "Invalid time.");

                            s.ScreeningStartingTime = $"{date} {time}";
                            return (true, null);
                        }
                    }
                }
            );

            return screeningEditor.Start();
        }

    public static void DeleteScreening()
    {
        Console.WriteLine();

        var idMapper = new Dictionary<string, Func<ScreeningModel, string>>
        {
            { "MovieId", x => MovieLogic.GetById((int)x.MovieId).Title }
        };

        var table = new TableUI<ScreeningModel>
        (
            "All screenings (Select any screening to delete)",
            new(
                [
                    new("ScreeningId", "Screening ID"),
                    new("MovieId", "Movie"),
                    new("HallId", "Hall ID"),
                    new("ScreeningStartingTime", "Start Time")
                ]
            ),
            allscreenings,
            ["HallId"],
            idMapper
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
        var idMapper = new Dictionary<string, Func<ScreeningModel, string>>
        {
            { "MovieId", x => MovieLogic.GetById((int)x.MovieId).Title }
        };

        var table = new TableUI<ScreeningModel>
        (
            "All screenings",
            new(
                [
                    new("ScreeningId", "Screening ID"),
                    new("MovieId", "Movie"),
                    new("HallId", "Hall ID"),
                    new("ScreeningStartingTime", "Start Time")
                ]
            ),
            allscreenings,
            ["HallId"],
            idMapper
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
