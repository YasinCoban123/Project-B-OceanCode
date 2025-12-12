static class Reservations
{
    static private UserLogic userLogic = new();

    public static void Start()
    {
        Console.Clear();

        var currentUser = AccountsLogic.CurrentAccount;
        if (currentUser == null)
        {
            Console.WriteLine("No user logged in.");
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
            Console.Clear();
            Menu.Start();
            return;
        }

        var reservations = userLogic.GetReservationsForUser(currentUser.AccountId).ToList();

        if (!reservations.Any())
        {
            Console.WriteLine("No reservations have yet been made");
            Console.WriteLine("Press enter to return to menu");
            Console.ReadLine();
            Console.Clear();
            Menu.Start();
            return;
        }


        Console.Clear();

        List<ReservationModel> reservationsList = new List<ReservationModel>();
        foreach (var r in reservations)
        {
            // string movieTitle = r.MovieTitle ?? "Unknown";
            // string screeningTime = r.ScreeningStartingTime ?? "N/A"; // laat gewoon als string
            // string seat = $"{r.RowNumber}-{r.SeatNumber}";

            // Console.WriteLine($"{r.ReservationId}\t{movieTitle,-20}\t{screeningTime,-20}\t{seat}");

            reservationsList.Add((ReservationModel)r);
        }

        Console.WriteLine();
        Console.WriteLine("Type a reservation ID to delete it, or type 'exit' or '0' to return to the main menu.");
        while (true)
        {
            var table = new TableUI<ReservationModel>(
                "Your reservations", 
                new(
                    [new("MovieTitle", "MovieTitle"),
                    new("ScreeningStartingTime", "Start time"),
                    new("Seat", "Seat")
                ]),
                reservationsList,
                ["MovieTitle", "ScreeningStartingTime"]);
            ReservationModel chosen = table.Start();

            MenuHelper menu = new MenuHelper(new[]
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
                        Menu.Start();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Reservation ID not found. Please retry or type 'exit' or '0' to return to the main menu.");
                    }
                    break;
                case 1:
                    break;
                case 2:
                    AccountPage.Start(currentUser);
                    break;
            }
        }
    }
}
