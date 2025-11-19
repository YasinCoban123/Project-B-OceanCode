static class Menu
{
    static public void Start()
    {
        UserAccountModel? currentUser = AccountsLogic.CurrentAccount;

        string[] options = new[] { "Account", "View Screenings", "Reservations", "Quit Program" };
        int selected = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Welcome to the Main Menu");
            Console.WriteLine("Use Arrow keys to navigate, Enter to select. Press 0 or Esc to quit.");
            Console.WriteLine();

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {options[i]}");
                }
            }

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.UpArrow)
            {
                selected = (selected - 1 + options.Length) % options.Length;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                selected = (selected + 1) % options.Length;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                if (selected == 0)
                {
                    AccountPage.Start(currentUser);
                }
                else if (selected == 1)
                {
                    Screenings.Start();
                }
                else if (selected == 2)
                {
                    Reservations.Start();
                }
                else if (selected == 3)
                {
                    Environment.Exit(0);
                }
            }
            else if (key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.D0 || key.Key == ConsoleKey.NumPad0)
            {
                Environment.Exit(0);
            }
        }
    }
    
        static public void AdminStart()
    {
        UserAccountModel? currentUser = AccountsLogic.CurrentAccount;
        ReservationAdmin radmin = new();

        string[] options = new[] { "Account", "View Screenings", "Reservations", "Movies", "Quit Program" };
        int selected = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Welcome to the Main Menu");
            Console.WriteLine("Use Arrow keys to navigate, Enter to select. Press 0 or Esc to quit.");
            Console.WriteLine();

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {options[i]}");
                }
            }

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.UpArrow)
            {
                selected = (selected - 1 + options.Length) % options.Length;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                selected = (selected + 1) % options.Length;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                if (selected == 0)
                {
                    AdminAccountPage.Start(currentUser);
                }
                else if (selected == 1)
                {
                    Console.WriteLine("No option for now </3");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                }
                else if (selected == 2)
                {
                    radmin.Start();
                }
                else if (selected == 3)
                {
                    Movie.Start();
                }
                else if (selected == 4)
                {
                    Environment.Exit(0);
                }
            }
            else if (key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.D0 || key.Key == ConsoleKey.NumPad0)
            {
                Environment.Exit(0);
            }
        }
    }
}