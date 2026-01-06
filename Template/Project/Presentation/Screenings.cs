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

        if (choice == 0)
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
            string selectedGenre = SelectGenreArrow();
            if (selectedGenre == null)
            {
                Console.WriteLine("\nPress ENTER to return...");
                Console.ReadLine();
                Console.Clear();
                MakeReservation();
                return;
            }

            filteredScreenings = screeningLogic.ShowScreeningsByGenre(selectedGenre);

            Console.Clear();
            if (filteredScreenings.Count == 0)
            {
                Console.WriteLine("No screenings found for this genre.");
                Console.WriteLine("\nPress ENTER to return...");
                Console.ReadLine();
                Console.Clear();
                MakeReservation();
                return;
            }

            ShowScreenings(filteredScreenings);

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
            Days? selectedDay = SelectDayArrow();
            if (selectedDay == null)
            {
                Console.WriteLine("\nPress ENTER to return...");
                Console.ReadLine();
                Console.Clear();
                MakeReservation();
                return;
            }

            filteredScreenings = screeningLogic.ShowScreeningsByDay(selectedDay.Value);

            Console.Clear();
            if (filteredScreenings.Count == 0)
            {
                Console.WriteLine("No screenings found on this day.");
                Console.WriteLine("\nPress ENTER to return...");
                Console.ReadLine();
                Console.Clear();
                MakeReservation();
                return;
            }

            ShowScreenings(filteredScreenings);

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
        else if (choice == 3)
        {
            Menu.Start();
            return;
        }

        int screeningId = SelectScreeningArrow(filteredScreenings);
        if (screeningId == -1)
        {
            Console.WriteLine("Press ENTER to return...");
            Console.ReadLine();
            Console.Clear();
            MakeReservation();
            return;
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

        var seatRows = GetSeatRowsOrReturn(screeningId);
        if (seatRows == null)
            return;

        PrintSeatLayoutHeader();
        PrintSeatRows(seatRows);
        PrintSeatLegend();

        int seatCount = 0;
        bool validInput = false;

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
            MenuHelper failedSeatMenu = new MenuHelper(
                [
                    "Continue with remaining seats",
                    "Cancel",
                    "Re-select failed seats"
                ],
                "Some seats could not be reserved:"
            );

            failedSeatMenu.Show();

            if (failedSeatMenu.SelectedIndex == 1)
            {
                Console.WriteLine("Press ENTER to return...");
                Console.ReadLine();
                Console.Clear();
                MakeReservation();
                return;
            }

            if (failedSeatMenu.SelectedIndex == 2)
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
            else
            {
                selectedSeatIds = selectedSeatIds.Except(failedSeats).ToList();
            }
        }

        decimal total = screeningLogic.CalculateTotalPrice(selectedSeatIds);
        Console.WriteLine($"\nTotal price: €{total:F2}");

        MenuHelper confirmMenu = new MenuHelper(
            ["Yes", 
            "No"
            ],
            @$"\nTotal price: €{total:F2}
            Confirm reservation:"
        );

        confirmMenu.Show();

        if (confirmMenu.SelectedIndex == 1)
        {
            AudioLogic.PlayReservationSuccessSound();
            Console.WriteLine("Press ENTER to return...");
            Console.ReadLine();
            Console.Clear();
            MakeReservation();
            return;
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
        {
            Console.WriteLine(s);
            Console.WriteLine();
        }
    }

    public static string SelectGenreArrow()
    {
        var genres = genreLogic.GetAllGenres();
        if (genres.Count == 0)
            return null;

        MenuHelper menu = new MenuHelper(genres, "Select a genre:");
        menu.Show();

        return genres[menu.SelectedIndex];
    }
    

    private static Days? SelectDayArrow()
    {
        string[] days = Enum.GetNames(typeof(Days));

        MenuHelper menu = new MenuHelper(days, "Select a day:");
        menu.Show();

        return (Days)menu.SelectedIndex;
    }

    private static int SelectScreeningArrow(List<string> screenings)
    {
        if (screenings.Count == 0)
            return -1;

        List<string> spaced = new List<string>();
        foreach (var s in screenings)
        {
            spaced.Add(s);
            spaced.Add("");
        }

        MenuHelper menu = new MenuHelper(spaced, "Select a screening:");
        menu.Show();

        string selected = spaced[menu.SelectedIndex];

        while (string.IsNullOrWhiteSpace(selected))
        {
            menu.Show();
            selected = spaced[menu.SelectedIndex];
        }

        string idPart = selected.Split(',')[0];
        int id = Convert.ToInt32(idPart.Replace("ScreeningId:", "").Trim());

        return id;
    }

    public static List<SeatRowLogic> GetSeatRowsOrReturn(int screeningId)
    {
        var seatRows = screeningLogic.GetSeatStatus(screeningId);

        if (seatRows == null || seatRows.Count == 0)
        {
            Console.WriteLine("No seats found for this screening.");
            Console.WriteLine("Press ENTER to return...");
            Console.ReadLine();
            Console.Clear();
            MakeReservation();
            return null;
        }

        return seatRows;
    }

    private static void PrintSeatLayoutHeader()
    {
        Console.Clear();
        Console.WriteLine("\nSeat Layout:\n");
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("                                                      ━━━━━━━━━━━━━━━━━━━━━━━━━ Screen ━━━━━━━━━━━━━━━━━━━━━━━━━\n");
        Console.ResetColor();
    }

    public static void PrintSeatRows(List<SeatRowLogic> seatRows)
    {
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
                PrintSeat(seat);
            }

            Console.WriteLine();
        }
    }

    private static void PrintSeat((long SeatId, int SeatNumber, string TypeName, decimal Price, bool IsTaken) seat)
    {
        if (seat.IsTaken)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[XXX]");
        }
        else
        {
            if (seat.TypeName == "Normal")
                Console.ForegroundColor = ConsoleColor.Green;
            else if (seat.TypeName == "Relax")
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.DarkMagenta;

            Console.Write($"[{seat.SeatId}]");
        }

        Console.ResetColor();
    }

    private static void PrintSeatLegend()
    {
        Console.WriteLine("\nLegend: [X] = Taken   [SeatId] = Free\n");

        Console.Write("Relax Color: ");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Relax");
        Console.ResetColor();

        Console.Write("VIP Color: ");
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("VIP");
        Console.ResetColor();
    }
}
