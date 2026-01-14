public class ReservationAdmin
{
    static private ScreeningLogic screeningLogic = new ScreeningLogic();
    static private GenreLogic genrelLogic = new GenreLogic();
    static private MovieLogic movieLogic = new MovieLogic();
    static private UserLogic userLogic = new();
    static private HallLogic hallLogic = new();

    public void Start()
    {
        Console.WriteLine();
        MenuHelper menu = new MenuHelper(new[]
        {
            "See all Reservations",
            "See statistics reserved Movies",
            "Go back"
        },
        "Reservation Admin");
        menu.Show();

        if (menu.SelectedIndex == 0)
        {
            List<ReservationModel> allReservations = screeningLogic.GetAllReservations();

            if (allReservations.Count == 0)
            {
                new MenuHelper(new[]
                    {
                        "Go Back"
                    },
                    "There are no reservations").Show();;
            }
            else
            {
                while (true)
                {
                    var table = new TableUI<ReservationModel>(
                    "All reservations", 
                    new(
                        [new("ReservationId", "ID"),
                        new("AccountId", "User ID"),
                        new("ScreeningId", "Screening ID"),
                        new("ReservationTime", "Time")
                        ]),
                        allReservations,
                        ["AccountId", "ScreeningId", "ReservationTime"]);
                    ReservationModel? chosen = table.Start();

                    if (chosen is null)
                    {
                        break;
                    }
    
                    menu = new MenuHelper(new[]
                    {
                        "Delete reservation",
                        "Go Back",
                        "Go back to main menu"
                    },
                    "Reservation");
                    menu.Show();
    
                    switch (menu.SelectedIndex)
                    {
                        case 0:
                            bool deleted = userLogic.DeleteReservationIfExists(chosen.ReservationId);
                            if (deleted)
                            {
                                Console.WriteLine("Reservation deleted.");
                                Console.WriteLine("Returning to main menu...");
                                Console.WriteLine("Press ENTER to continue");
                                Console.ReadLine();
                                Console.Clear();
                            }
                            break;
                        case 1:
                            break;
                        case 2:
                            Start();
                            break;
                    }                   
                }
            }
        }

        if (menu.SelectedIndex == 1)
        {
            Console.Clear();

            MenuHelper menu2 = new MenuHelper(new[]
            {
                "View statistics between two dates",
                "View All round statistics",
                "Return back to Menu"
            },
            "Screenings Admin");

            menu2.Show();

            int choice = menu2.SelectedIndex;

            Console.Clear();

            if (choice == 0)
            {
                ViewStatsFiltered();
                Console.Clear();
            }
            else if (choice == 1)
            {
                ViewStats();
                Console.Clear();
            }
            else if (choice == 2)
            {
                Console.Clear();
                Start();
            }
        }
        if (menu.SelectedIndex == 2)
        {
            Console.Clear();
            Menu.AdminStart();
        } 
    }

    public static void ViewStatsFiltered()
    {
        Console.WriteLine("Enter first date (dd-MM-yyyy): ");
        string date1 = Console.ReadLine()!;

        Console.WriteLine("Enter second date (dd-MM-yyyy): ");
        string date2 = Console.ReadLine()!;

        bool res = userLogic.FilterBetweenCheck(date1, date2);
        if(!res)
        {
            Console.WriteLine("Dates given not correct");
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
            Console.Clear();
            ViewStatsFiltered();
            return;
        }

        DateTime fromDate = DateTime.Parse(date1);
        DateTime toDate = DateTime.Parse(date2);

        ViewStatsBetween(fromDate, toDate);
    }

    public static void ViewStatsBetween(DateTime fromDate, DateTime toDate)
    {
        Console.Clear();
        Console.WriteLine("=== Statistics for Reserved Movies ===\n");

        Console.WriteLine("Date with most reservations:");
        string res = userLogic.GetDateWithMostReservationsBetween(fromDate, toDate);

        if (res == null)
        {
            PrintResultMessage();
        }
        else
        {
            PrintResultMessage(res);
        }

        Console.WriteLine("\nPress ENTER to go back...");
        Console.ReadLine();
        Console.Clear();
    }

    public static void ViewStats()
    {
        Console.Clear();
        Console.WriteLine("=== Statistics for Reserved Movies ===\n");
        
        Console.WriteLine($"Most popular movie:");
        string res1 = movieLogic.GetMostPopularMovie();
        if (res1 == null)
        {
            PrintResultMessage();
        }
        else
        {
            PrintResultMessage(res1);
        }

        Console.WriteLine($"Most popular genre:");
        string res2 = genrelLogic.GetMostPopularGenre();
        if (res2 == null)
        {
            PrintResultMessage();
        }
        else
        {
            PrintResultMessage(res2);
        }

        Console.WriteLine($"Date with most reservations:");
        string res3 = userLogic.GetDateWithMostReservations();
        if (res3 == null)
        {
            PrintResultMessage();
        }
        else
        {
            PrintResultMessage(res3);
        }
            
        Console.WriteLine("\nPress ENTER to go back...");
        Console.ReadLine();
        Console.Clear();
        Menu.AdminStart();
    }

    static void PrintResultMessage()
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("No data available, so no reservations has been made");
        Console.ResetColor();
    }

    static void PrintResultMessage(string res)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine(res);
        Console.ResetColor();
    }      
}
