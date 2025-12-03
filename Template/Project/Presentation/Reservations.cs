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
    Console.WriteLine("Your reservations:");
    Console.WriteLine("ID\tMovie Title\t\tScreening Time\t\tSeat");
    
    foreach (var r in reservations)
    {
        string movieTitle = r.MovieTitle ?? "Unknown";
        string screeningTime = r.ScreeningStartingTime ?? "N/A"; // laat gewoon als string
        string seat = $"{r.RowNumber}-{r.SeatNumber}";
    
        Console.WriteLine($"{r.ReservationId}\t{movieTitle,-20}\t{screeningTime,-20}\t{seat}");
    }



        Console.WriteLine();
        Console.WriteLine("Type a reservation ID to delete it, or type 'exit' or '0' to return to the main menu.");
        while (true)
        {
            Console.Write("Choice: ");
            string input = Console.ReadLine();
            if (input != null && (input.ToLower() == "exit" || input == "0"))
            {
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
                Console.Clear();
                Menu.Start();
                return;
            }

            if (!long.TryParse(input, out long reservationId))
            {
                Console.WriteLine("Invalid ID. Please enter a numeric reservation ID or 'exit' or '0' to return to the main menu.");
                continue;
            }

            if (!reservations.Any(r => r.ReservationId == reservationId))
            {
                Console.WriteLine("You may only delete your own reservations. Please enter a valid reservation ID from the list or type 'exit'.");
                continue;
            }

            bool deleted = userLogic.DeleteReservationIfExists(reservationId);
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
        }
    }
}
