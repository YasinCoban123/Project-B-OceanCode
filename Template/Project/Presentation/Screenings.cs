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
                validInput = true;
            }
            catch
            {
                Console.WriteLine("Invalid input. Please enter a number or 'exit'.");
            }
        }

        Console.WriteLine("\nSeat layout for this hall (for this screening):");
        var seatStatus = screeningLogic.GetSeatStatus(screeningId);

        if (seatStatus.Count == 0)
        {
            Console.WriteLine("No seats found for this screening.");
            Menu.Start();
            return;
        }

        foreach (var s in seatStatus)
        {
            string status = s.IsTaken ? "[TAKEN]" : "[FREE]";
            Console.WriteLine($"SeatId: {s.SeatId} | Row: {s.RowNumber} | Number: {s.SeatNumber} | Type: {s.TypeName} | Price: {s.Price} | {status}");
        }

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
