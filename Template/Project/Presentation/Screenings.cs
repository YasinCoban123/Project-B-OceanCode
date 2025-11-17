static class Screenings
{
    static private ScreeningLogic screeningLogic = new ScreeningLogic();

    public static void Start()
    {
        var currentUser = AccountsLogic.CurrentAccount;
        Console.WriteLine($"Welcome to the screenings page, {currentUser.FullName}");

        var screenings = screeningLogic.ShowScreenings();
        foreach (var s in screenings)
            Console.WriteLine(s);

        List<int> validScreeningIds = new List<int>();
        foreach (string scr in screenings)
        {
            string idPart = scr.Split(',')[0];
            int id = Convert.ToInt32(idPart.Replace("ScreeningId:", "").Trim());
            validScreeningIds.Add(id);
        }

        Console.WriteLine("\nType 'G' to filter by Genre, 'D' to sort by Date, or press Enter to continue normally.");
        string filterChoice = Console.ReadLine()!.Trim().ToUpper();

        if (filterChoice == "G")
        {
            Console.Write("\nEnter genre to filter by: ");
            Console.WriteLine("\nAvailable genres: ");
            string[] values = Enum.GetNames(typeof(Genres));
            foreach (string word in values)
                Console.WriteLine(word);

            string genre = Console.ReadLine()!;
            List<string> filtered = screeningLogic.ShowScreeningsByGenre(genre);

            if (filtered.Count == 0)
            {
                Console.WriteLine("\nNo screenings found for this genre. Returning to screenings...");
                Start();
                return;
            }

            validScreeningIds.Clear();
            foreach (string s in filtered)
            {
                Console.WriteLine(s);
                string idPart = s.Split(',')[0];
                int id = Convert.ToInt32(idPart.Replace("ScreeningId:", ""));
                validScreeningIds.Add(id);
            }
        }
        else if (filterChoice == "D")
        {
            List<string> sorted = screeningLogic.ShowScreeningsByDate();
            Console.WriteLine("\nScreenings sorted by date:");  // ← oude WriteLine toegevoegd

            validScreeningIds.Clear();
            foreach (string s in sorted)
            {
                Console.WriteLine(s);
                string idPart = s.Split(',')[0];
                int id = Convert.ToInt32(idPart.Replace("ScreeningId:", ""));
                validScreeningIds.Add(id);
            }
        }

        int screeningId = 0;
        bool validInput = false;

        while (!validInput)
        {
            Console.Write("\nEnter the ScreeningId you want to reserve (or type 'exit' to go back): ");
            string input = Console.ReadLine();

            if (input == null)
                continue;

            if (input.ToLower() == "exit")
            {
                Console.WriteLine("\nReturning to menu...");
                Menu.Start();
                return;
            }

            try
            {
                screeningId = Convert.ToInt32(input);

                if (validScreeningIds.Contains(screeningId))
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("This screening is not available based on your selection. Try again.");
                }
            }
            catch
            {
                Console.WriteLine("Invalid input. Please enter a number or 'exit'.");
            }
        }

        Console.WriteLine("\nSeat layout for this hall (for this screening):");

        var seatRows = screeningLogic.GetSeatStatus(screeningId);

        if (seatRows == null || seatRows.Count == 0)
        {
            Console.WriteLine("No seats found for this screening.");
            Start();
            return;
        }

        Console.WriteLine("\nSeat Layout:\n");
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("         ━━━━━━━━━━━━━━━━━━━━━━━━━ Screen ━━━━━━━━━━━━━━━━━━━━━━━━━\n");
        Console.ResetColor();

        foreach (var row in seatRows)
        {
            Console.Write($"Row {row.RowNumber}: ");

            foreach (var seat in row.Seats)
            {
                if (seat.IsTaken)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[X]");
                }
                else
                {
                    if (seat.TypeName == "Normal")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"[{seat.SeatId}]");
                    }
                    else if (seat.TypeName == "Relax")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"[{seat.SeatId}]");
                    }
                    else 
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write($"[{seat.SeatId}]");
                    }
                }
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        Console.WriteLine("\nLegend: [X] = Taken   [ ] = Free\n");
        Console.Write("Relax Color: ");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Relax");
        Console.ResetColor();
        Console.Write("VIP Color: ");
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("VIP");
        Console.ResetColor();
        int seatCount = 0;
        validInput = false;
        while (!validInput)
        {
            Console.Write("\nHow many seats do you want to reserve (max 20)? ");
            try
            {
                seatCount = Convert.ToInt32(Console.ReadLine());
                if (seatCount > 0 && seatCount <= 20)
                    validInput = true;
                else
                    Console.WriteLine("You can reserve between 1 and 20 seats.");
            }
            catch
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }

        List<int> selectedSeatIds = new();
        for (int i = 0; i < seatCount; i++)
        {
            int seatId = 0;
            bool seatValid = false;
            while (!seatValid)
            {
                Console.Write($"Enter SeatId #{i + 1}: ");
                try
                {
                    seatId = Convert.ToInt32(Console.ReadLine());
                    seatValid = true;
                }
                catch
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
            selectedSeatIds.Add(seatId);
        }

        var (valid, failedSeats) = screeningLogic.ValidateSeatsBeforeReservation(currentUser, screeningId, selectedSeatIds);

        if (failedSeats.Count == selectedSeatIds.Count)
        {
            Console.WriteLine("\nAll selected seats are unavailable. Please try again.");
            Start();
            return;
        }

        if (failedSeats.Count > 0)
        {
            Console.WriteLine("\nSome seats could not be reserved:");
            foreach (var fs in failedSeats)
                Console.WriteLine($" - Seat {fs} (either does not exist or is already taken)");

            Console.Write("\nDo you want to continue with the remaining available seats? (yes/no): ");
            string? choice = Console.ReadLine()?.ToLower();

            if (choice != "yes")
            {
                Console.WriteLine("Returning to the screenings page...");
                Start();
                return;
            }

            selectedSeatIds = selectedSeatIds.Except(failedSeats).ToList();
        }

        if (selectedSeatIds.Count == 0)
        {
            Console.WriteLine("\nNo valid seats selected. Returning to menu...");
            Menu.Start();
            return;
        }

        screeningLogic.ConfirmReservation(currentUser, screeningId, selectedSeatIds);
        Console.WriteLine("\nReservation successful!");

        Menu.Start();
    }
}
