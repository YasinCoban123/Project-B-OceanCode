static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        UserAccountModel? currentUser = AccountsLogic.CurrentAccount;

        OptionsMenu optionsMenu;

        while (true)
        {
            optionsMenu = new OptionsMenu(new() {
                "Account",
                "View Screenings",
                "Reservations",
                "Log out",
                "Quit"}
                , "Welcome to the Main Menu");

            switch (optionsMenu.Selected)
            {
                case 0:
                    AccountPage.Start(currentUser);
                    break;
                case 1:
                    Screenings.MakeReservation();
                    break;
                case 2:
                    Reservations.Start();
                    break;
                case 3:
                    //Log out
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
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
            Console.WriteLine($"[2] Screenings");
            Console.WriteLine($"[3] Reservations");
            Console.WriteLine("[4] Movies");
            Console.WriteLine("[5] Quit Program");
            string answer = Console.ReadLine().ToLower();

            if (answer == "1")
            {
                AdminAccountPage.Start(currentUser);
            }
            else if (answer == "2")
            {
                Screenings.Start();
            }
            else if (answer == "3")
            {
                radmin.Start();
            }

            else if (answer == "4")
            {
                Movie.Start();
            }   

            else if (answer == "5")
            {
                Environment.Exit(0);
            }
        }
    }
}