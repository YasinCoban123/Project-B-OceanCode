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

        bool success = screeningLogic.MakeReservation(currentUser.AccountId, screeningId, selectedSeatIds);
        Console.WriteLine(success ? "\nReservation successful!" : "\nFailed to make reservation.");

        Menu.Start();
    }
}
