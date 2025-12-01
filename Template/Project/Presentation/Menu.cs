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

        OptionsMenu optionsMenu;

        while (true)
        {
            optionsMenu = new OptionsMenu(new() {
                "Account",
                "Screenings",
                "Reservations",
                "Movies",
                "Log out",
                "Quit"}
                , "Welcome to the Admin menu");

            switch (optionsMenu.Selected)
            {
                case 0:
                    AdminAccountPage.Start(currentUser);
                    break;
                case 1:
                    Screenings.Start();
                    break;
                case 2:
                    Reservations.Start();
                    break;
                case 3:
                    radmin.Start();
                    break;
                case 4:
                    //Log out
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}