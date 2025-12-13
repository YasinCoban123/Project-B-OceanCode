using System;
using System.Collections.Generic;
using System.Linq;

static class Screenings
{
    static private ScreeningLogic screeningLogic = new ScreeningLogic();
    static private GenreLogic genreLogic = new GenreLogic();

    public static void MakeReservation()
    {
        Console.Clear();
        var currentUser = AccountsLogic.CurrentAccount;

        MenuHelper menu = new MenuHelper(new[]
        {
            "Show all screenings",
            "Filter by genre",
            "Filter by day",
            "Go back"
        },
        $"Welcome {currentUser.FullName}, choose an option:");

        menu.Show();
        int choice = menu.SelectedIndex;
        Console.Clear();

        List<string> allScreenings = screeningLogic.ShowScreenings();
        List<string> filteredScreenings = allScreenings;

        //var table = new TableUI()

        while (filtering)
        {
            ShowScreenings(allScreenings);

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();

            Console.WriteLine("\nType 'back' to return to screenings menu, or press ENTER to continue:");
            string ans = Console.ReadLine()!.Trim().ToLower();
            if (ans == "back")
            {
                Console.Clear();
                MakeReservation();
                return;
            }

            filteredScreenings = allScreenings;
        }
        else if (choice == 1)
        {
            filteredScreenings = FilterByGenre(allScreenings);

            if (filteredScreenings.Count == 0)
            {
                Console.WriteLine("\nPress ENTER to return...");
                Console.ReadLine();
                Console.Clear();
                MakeReservation();
                return;
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();

            Console.WriteLine("\nType 'back' to return to screenings menu, or press ENTER to continue:");
            string ans = Console.ReadLine()!.Trim().ToLower();
            if (ans == "back")
            {
                Console.Clear();
                MakeReservation();
                return;
            }
        }
        else if (choice == 2)
        {
            filteredScreenings = FilterByDay(allScreenings);

            if (filteredScreenings.Count == 0)
            {
                Console.WriteLine("\nPress ENTER to return...");
                Console.ReadLine();
                Console.Clear();
                MakeReservation();
                return;
            }

            Console.WriteLine("\nType 'back' to return to screenings menu, or press ENTER to continue:");
            string ans = Console.ReadLine()!.ToLower();
            if (ans == "back")
            {
                Console.Clear();
                MakeReservation();
                return;
            }
        }
        else if (choice == 3)
        {
            Menu.Start();
            return;
        }

        List<int> validScreeningIds = new List<int>();
        foreach (string scr in filteredScreenings)
        {
            string idPart = scr.Split(',')[0];
            int id = Convert.ToInt32(idPart.Replace("ScreeningId:", "").Trim());
            validScreeningIds.Add(id);
        }

        int screeningId = 0;
        bool validInput = false;

        while (!validInput)
        {
            Console.Write("\nEnter the ScreeningId you want to reserve (or type 'exit' to go back): ");
            string input = Console.ReadLine();

            if (input.ToLower() == "exit")
            {
                Console.WriteLine("Press ENTER to return...");
                Console.ReadLine();
                Console.Clear();
                MakeReservation();
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
                    Console.WriteLine("This screening is not available based on your selection.");
                }
            }
            catch
            {
                Console.WriteLine("Invalid input.");
            }
        }

        var accountsLogic = new AccountsLogic();
        if (screeningLogic.GetPGRatingByAccess(screeningId) > accountsLogic.getAge(currentUser.DateOfBirth))
        {
            Console.WriteLine("\nYou do not meet the age requirement for this movie screening.");
            Console.WriteLine("Press ENTER to return to menu...");
            Console.ReadLine();
            Console.Clear();
            Menu.Start();
            return;
        }

        Console.WriteLine("\nSeat layout for this hall (for this screening):");

        var seatRows = screeningLogic.GetSeatStatus(screeningId);

        if (seatRows == null || seatRows.Count == 0)
        {
            Console.WriteLine("No seats found for this screening.");
            Console.WriteLine("Press ENTER to return...");
            Console.ReadLine();
            Console.Clear();
            MakeReservation();
            return;
        }

        Console.WriteLine("\nSeat Layout:\n");
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("                                                      ━━━━━━━━━━━━━━━━━━━━━━━━━ Screen ━━━━━━━━━━━━━━━━━━━━━━━━━\n");
        Console.ResetColor();

        foreach (var row in seatRows)
        {
            string rowLabel = $"Row {row.RowNumber}: ";
            int totalWidth = Console.WindowWidth;

            string preview = "";
            foreach (var seat in row.Seats)
                preview += "[XXX]";

            int seatWidth = preview.Length;
            int leftPadding = (totalWidth - rowLabel.Length - seatWidth) / 2;
            if (leftPadding < 0) leftPadding = 0;

            Console.Write(rowLabel);
            Console.Write(new string(' ', leftPadding));

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
                Console.WriteLine("Invalid input.");
            }
        }

        List<int> selectedSeatIds = new();
        var validSeatIds = seatRows.SelectMany(r => r.Seats).Select(s => (int)s.SeatId).ToList();

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

                    if (!validSeatIds.Contains(seatId))
                    {
                        Console.WriteLine("That seat does not exist in this hall.");
                        continue;
                    }

                    if (selectedSeatIds.Contains(seatId))
                    {
                        Console.WriteLine("You already selected this seat. Choose another one.");
                        continue;
                    }

                    seatValid = true;
                }
                catch
                {
                    Console.WriteLine("Invalid input.");
                }
            }

            selectedSeatIds.Add(seatId);
        }

        var (valid, failedSeats) = screeningLogic.ValidateSeatsBeforeReservation(currentUser, screeningId, selectedSeatIds);

        if (failedSeats.Count == selectedSeatIds.Count)
        {
            Console.WriteLine("\nAll selected seats are unavailable. Please try again.");
            Console.WriteLine("Press ENTER to return...");
            Console.ReadLine();
            Console.Clear();
            MakeReservation();
            return;
        }

        if (failedSeats.Count > 0)
        {
            Console.WriteLine("\nSome seats could not be reserved:");
            foreach (var fs in failedSeats)
                Console.WriteLine($" - Seat {fs}");

            Console.WriteLine("\nChoose:");
            Console.WriteLine("[1] Continue with remaining seats");
            Console.WriteLine("[2] Cancel");
            Console.WriteLine("[3] Re-select failed seats");

            string choice2 = Console.ReadLine();

            if (choice2 == "2")
            {
                Console.WriteLine("Press ENTER to return...");
                Console.ReadLine();
                Console.Clear();
                MakeReservation();
                return;
            }

            if (choice2 == "3")
            {
                List<int> replacements = new List<int>();
                foreach (int fs in failedSeats)
                {
                    Console.Write($"Replace seat {fs} with: ");
                    replacements.Add(Convert.ToInt32(Console.ReadLine()));
                }

                selectedSeatIds = selectedSeatIds.Except(failedSeats).ToList();
                selectedSeatIds.AddRange(replacements);
            }
            else if (choice2 == "1")
            {
                selectedSeatIds = selectedSeatIds.Except(failedSeats).ToList();
            }
            else
            {
                Console.WriteLine("Press ENTER to return...");
                Console.ReadLine();
                Console.Clear();
                MakeReservation();
                return;
            }
        }

        decimal total = screeningLogic.CalculateTotalPrice(selectedSeatIds);
        Console.WriteLine($"\nTotal price: €{total:F2}");

        string confirm = "";
        bool answered = false;

        while (!answered)
        {
            Console.Write("Confirm? (yes/no): ");
            confirm = Console.ReadLine().Trim().ToLower();

            if (confirm == "yes")
            {
                answered = true;
            }
            else if (confirm == "no")
            {
                Console.WriteLine("Press ENTER to return...");
                Console.ReadLine();
                Console.Clear();
                MakeReservation();
                return;
            }
            else
            {
                Console.WriteLine("Invalid choice. Type yes or no.");
            }
        }

        screeningLogic.ConfirmReservation(currentUser, screeningId, selectedSeatIds);
        Console.WriteLine("Press ENTER to return to menu...");
        Console.ReadLine();
        Console.Clear();
        Menu.Start();
    }

    private static void ShowScreenings(List<string> screenings)
    {
        Console.WriteLine("=== Screenings ===\n");
        foreach (string s in screenings)
            Console.WriteLine(s);
    }

    private static List<string> FilterByGenre(List<string> all)
    {
        Console.WriteLine("Available genres: ");

        List<string> genres = genreLogic.GetAllGenres();

        foreach (string g in genres)
            Console.WriteLine(g);

        Console.Write("\nEnter genre to filter by: ");
        string genre = Console.ReadLine()!;

        Console.Clear();

        var shown = screeningLogic.ShowScreeningsByGenre(genre);

        if (shown.Count == 0)
        {
            Console.WriteLine("No screenings found for this genre.");
            return new List<string>();
        }

        foreach (var s in shown)
            Console.WriteLine(s);

        return shown;
    }

    private static List<string> FilterByDay(List<string> all)
    {
        Console.WriteLine("Available days:");
        string[] values = Enum.GetNames(typeof(Days));
        foreach (string word in values)
            Console.WriteLine(word);

        Console.Write("\nEnter day: ");
        string d = Console.ReadLine();

        Days chosen = Days.Monday;
        bool match = false;
        string[] names = Enum.GetNames(typeof(Days));

        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].ToLower() == d.ToLower())
            {
                chosen = (Days)i;
                match = true;
                break;
            }
        }

        if (!match)
        {
            Console.WriteLine("Invalid day.");
            return new List<string>();
        }

        Console.Clear();

        var shown = screeningLogic.ShowScreeningsByDay(chosen);

        if (shown.Count == 0)
        {
            Console.WriteLine("No screenings found on this day.");
            return new List<string>();
        }

        foreach (var s in shown)
            Console.WriteLine(s);

        return shown;
    }
}
