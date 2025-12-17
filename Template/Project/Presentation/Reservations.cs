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
            reservationsList.Add((ReservationModel)r);
        }

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
            ReservationModel? chosen = table.Start();

            if (chosen is null)
            {
                return;
            }

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

                        // Update data
                        reservations = userLogic.GetReservationsForUser(currentUser.AccountId).ToList();
                        reservationsList = new();
                        foreach (var r in reservations)
                        {
                            reservationsList.Add((ReservationModel)r);
                        }
                    }
                    break;
                case 1:
                    break;
                case 2:
                    Menu.Start();
                    break;
            }
        }
    }
}
