public class ReservationAdmin
{
    ScreeningLogic screeningLogic = new ScreeningLogic();
    GenreLogic genrelLogic = new GenreLogic();
    static private UserLogic userLogic = new();

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
            Console.WriteLine("All statistics from reserved movies");

        }
        if (menu.SelectedIndex == 2)
        {
            Console.Clear();
            Menu.AdminStart();
        }



    }
}