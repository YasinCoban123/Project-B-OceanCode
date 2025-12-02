static class Screenings
{
    static private ScreeningLogic screeningLogic = new ScreeningLogic();
    static private MovieLogic movielogic = new();
    static private HallLogic hallLogic = new();

    public static void Start()
    {
        OptionsMenu optionsMenu = new OptionsMenu(new() {
            "Add a screening",
            "Delete a screening"});

        if (optionsMenu.Selected == 0)
        {
            AddScreening();
        }
        else if (optionsMenu.Selected == 1)
        {
            DeleteScreening();
        }
        else
        {
            Console.WriteLine("Invalid choice");
        }
    }

    public static void MakeReservation()
    {
        var currentUser = AccountsLogic.CurrentAccount;
        Console.WriteLine($"Welcome to the screenings page, {currentUser.FullName}");

        var screenings = screeningLogic.ShowScreenings();
        List<string> shown = screenings;

        bool filtering = true;

        while (filtering)
        {
            Console.WriteLine("\nCurrent screenings:");
            foreach (var s in shown)
                Console.WriteLine(s);

            Console.WriteLine("\nFilter options:");
            Console.WriteLine("[G] Filter by Genre");
            Console.WriteLine("[Y] Filter by Day");
            Console.WriteLine("[N] No filter");
            Console.WriteLine("[C] Continue");

            Console.Write("\nChoose: ");
            string filterChoice = Console.ReadLine()!.Trim().ToUpper();

            if (filterChoice == "G")
            {
                Console.WriteLine("\nAvailable genres: ");
                string[] values = Enum.GetNames(typeof(Genres));
                foreach (string word in values)
                    Console.WriteLine(word);

                Console.Write("\nEnter genre to filter by: ");
                string genre = Console.ReadLine()!;
                shown = screeningLogic.ShowScreeningsByGenre(genre);

                if (shown.Count == 0)
                {
                    Console.WriteLine("No screenings found for this genre.");
                    shown = screenings;
                }
            }
            else if (filterChoice == "Y")
            {
                Console.WriteLine("\nAvailable days:");
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
                    continue;
                }

                shown = screeningLogic.ShowScreeningsByDay(chosen);

                if (shown.Count == 0)
                {
                    Console.WriteLine("No screenings found on this day.");
                    shown = screenings;
                }
            }
            else if (filterChoice == "N")
            {
                shown = screenings;
            }
            else if (filterChoice == "C")
            {
                filtering = false;
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        List<int> validScreeningIds = new List<int>();
        foreach (string scr in shown)
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
                Console.WriteLine("Press ENTER to go back");
                Console.ReadLine();
                Console.Clear();
                Menu.Start();
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
                Console.WriteLine("Invalid input.");
            }
        }

        Console.WriteLine("\nSeat layout for this hall (for this screening):");

        var seatRows = screeningLogic.GetSeatStatus(screeningId);

        if (seatRows == null || seatRows.Count == 0)
        {
            Console.WriteLine("No seats found for this screening.");
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

            string seatTextPreview = "";
            foreach (var seat in row.Seats)
            {
                seatTextPreview += "[XXX]";
            }

            int seatWidth = seatTextPreview.Length;
            int leftPadding = (totalWidth - rowLabel.Length - seatWidth) / 2;
            if (leftPadding < 0)
            {
                leftPadding = 0;
            }
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
                        Console.WriteLine("That seat does not exist in this hall. Try again.");
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

            string choice = Console.ReadLine();

            if (choice == "2")
            {
                MakeReservation();
                return;
            }

            if (choice == "3")
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
            else if (choice == "1")
            {
                selectedSeatIds = selectedSeatIds.Except(failedSeats).ToList();
            }
            else
            {
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

    public static void AddScreening()
    {
        List<MovieModel> allmovies = movielogic.GetAllMovies();
        List<HallModel> allhalls = hallLogic.GetAllHalls();
        Console.WriteLine();
        Console.WriteLine("Welcome to the add screening page");
        foreach (MovieModel movie in allmovies)
        {
            Console.WriteLine();
            Console.WriteLine($"Movie ID: {movie.MovieId}");
            Console.WriteLine($"Movie Title: {movie.Title}");
        }
        Console.WriteLine("Choose above which movie ID you want to add to the screening");
        string chosenMovieid = Console.ReadLine();
        int chosenMovieID = Convert.ToInt32(chosenMovieid);
        MovieModel ChosenMovie = allmovies.Find(Movie => chosenMovieID == Movie.MovieId);
        long MovieID = ChosenMovie.MovieId;
        Console.WriteLine();
        Console.WriteLine("Which Hall do you want the screening to be in\nHall 1\nHall 2\nHall 3");
        string chosenHallid = Console.ReadLine();
        long ChosenHallID = Convert.ToInt64(chosenHallid);
        HallModel ChosenHall = allhalls.Find(Hall => ChosenHallID == Hall.HallId);
        long HallID = ChosenHall.HallId;
        string Date = "";
        string Time = "";
        bool Datebool = true;
        while (Datebool)
        {
            Console.WriteLine();
            Console.Write("Give a Date when the screening should be played: ");
            Date = Console.ReadLine();
            if (hallLogic.CheckDate(Date))
            {
                break;
            }
            Console.WriteLine("Invalid Date");
        }

        bool Timebool = true;
        while (Timebool)
        {
            Console.WriteLine();
            Console.Write("Give a Time when the screening should be played: ");
            Time = Console.ReadLine();
            if (hallLogic.CheckTime(Time))
            {
                break;
            }
            Console.WriteLine("Invalid Time");
        }
        string DateTime = $"{Date} {Time}";
        screeningLogic.AddScreening(MovieID, HallID, DateTime);
        Console.WriteLine();
        Console.WriteLine("Screening Succesfully added!");
    }

    public static void DeleteScreening()
    {
        List<string> screenings = screeningLogic.ShowScreenings();
        foreach (var s in screenings)
        {
            Console.WriteLine(s);
        }

        Console.WriteLine("Choose a screening to delete");
        string choice = Console.ReadLine();
        long choiceint = Convert.ToInt64(choice);
        screeningLogic.DeleteScreening(choiceint);
    }
}
