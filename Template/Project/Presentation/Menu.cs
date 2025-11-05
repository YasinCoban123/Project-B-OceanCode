static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        UserAccountModel? currentUser = AccountsLogic.CurrentAccount;

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Welcome to the Main Menu");
            Console.WriteLine("[1] Account");
            Console.WriteLine("[2] View Screenings");
            Console.WriteLine("[3] Reservations");
            Console.WriteLine("[4] Quit Program");
            string answer = Console.ReadLine().ToLower();

            if (answer == "1")
            {
                AccountPage.Start(currentUser);
            }
            else if (answer == "2")
            {
                Screenings.Start();
            }
            else if (answer == "3")
            {
                Reservations.Start();
            }
            else if (answer == "4")
            {
                Environment.Exit(0);
            }
        }
    }
    
        static public void AdminStart()
    {
        UserAccountModel? currentUser = AccountsLogic.CurrentAccount;
        ReservationAdmin radmin = new();
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"Welcome to the Main Menu");
            Console.WriteLine($"[1] Account");
            Console.WriteLine($"[2] View Screenings");
            Console.WriteLine($"[3] Reservations");
            Console.WriteLine("[4] Quit Program");
            string answer = Console.ReadLine().ToLower();

            if (answer == "1")
            {
                AdminAccountPage.Start(currentUser);
            }
            else if (answer == "2")
            {
                Console.WriteLine("No option for now </3");
            }
            else if (answer == "3")
            {
                radmin.Start();
            }

            else if (answer == "4")
            {
                Environment.Exit(0);
            }
        }
    }
}