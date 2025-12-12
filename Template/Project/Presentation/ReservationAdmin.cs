public class ReservationAdmin
{
    ScreeningLogic screeningLogic = new ScreeningLogic();

    public void Start()
    {
        Console.WriteLine();
        MenuHelper menu = new MenuHelper(new[]
        {
            "See all Reservations",
            "Go Back"
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
                var table = new TableUI<ReservationModel>(
                "All reservations (Select any to go back)", 
                new(
                    [new("ReservationId", "ID"),
                    new("AccountId", "User ID"),
                    new("ScreeningId", "Screening ID"),
                    new("ReservationTime", "Time")
                    ]),
                    allReservations,
                    ["AccountId", "ScreeningId", "ReservationTime"]);
                table.Start();
            }
        }

    }
}