using System;
using System.Linq;

static class Reservations
{
    static private UserLogic userLogic = new();

    public static void Start()
    {
        var currentUser = AccountsLogic.CurrentAccount;
        if (currentUser == null)
        {
            Console.WriteLine("No user logged in.");
            Menu.Start();
            return;
        }

        var reservations = userLogic.GetReservationsForUser(currentUser.AccountId).ToList();

        if (!reservations.Any())
        {
            Console.WriteLine("No reservations have yet been made");
            Menu.Start();
            return;
        }

        Console.WriteLine("Your reservations:");
        Console.WriteLine("ID\tScreeningId\tTime");
        foreach (var r in reservations)
        {
            Console.WriteLine($"{r.ReservationId}\t{r.ScreeningId}\t{r.ReservationTime}");
        }

        Console.WriteLine();
        Console.WriteLine("Type a reservation ID to delete it, or type 'exit' to return to the main menu.");
        while (true)
        {
            Console.Write("Choice: ");
            string input = Console.ReadLine();
            if (input != null && input.ToLower() == "exit")
            {
                Menu.Start();
                return;
            }

            if (!long.TryParse(input, out long reservationId))
            {
                Console.WriteLine("Invalid ID. Please enter a numeric reservation ID or 'exit'.");
                continue;
            }

            bool deleted = userLogic.DeleteReservationIfExists(reservationId);
            if (deleted)
            {
                Console.WriteLine("Reservation deleted.");
                Menu.Start();
                return;
            }
            else
            {
                Console.WriteLine("Reservation ID not found. Please retry or type 'exit'.");
            }
        }
    }
}
