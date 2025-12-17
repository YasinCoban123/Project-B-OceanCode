static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        AudioLogic.PlayBackgroundMusic();
        UserAccountModel? currentUser = AccountsLogic.CurrentAccount;

        MenuHelper menu;

        while (true)
        {
            menu = new MenuHelper(new[] {
                "Account",
                "View Screenings",
                "Reservations",
                "Log out",
                "Quit"
            },
            "Welcome to the Main Menu");

            menu.Show();

            switch (menu.SelectedIndex)
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
                    UserLogin.Start();
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

        MenuHelper menu;

        while (true)
        {
            menu = new MenuHelper(new[] {
                "Account",
                "Screenings",
                "Reservations",
                "Movies",
                "Hall",
                "Log out",
                "Quit"
            },
            "Welcome to the Admin menu");

            menu.Show();

            switch (menu.SelectedIndex)
            {
                case 0:
                    AdminAccountPage.Start(currentUser);
                    break;
                case 1:
                    ScreeningsAdmin.Start();
                    break;
                case 2:
                    radmin.Start();
                    break;
                case 3:
                    Movie.Start();
                    break;
                case 4:
                    Hall.Start();
                    break;
                case 5:
                    UserLogin.Start();
                    break;
                case 6:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}