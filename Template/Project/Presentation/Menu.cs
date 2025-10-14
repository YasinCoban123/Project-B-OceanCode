static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        UserAccountModel ?currentUser = AccountsLogic.CurrentAccount;
        while (true)
        {
            Console.WriteLine($"Welcome to the Main Menu {currentUser.FullName}");
            Console.WriteLine($"Account");
            Console.WriteLine($"View Screenings");
            Console.WriteLine($"Reservations");
            string answer = Console.ReadLine().ToLower();

            if (answer == "account")
            {
                AccountPage.Start(currentUser);
            }
            else if (answer == "view screenings")
            {
                Screenings.Start();
            }
            else if (answer == "reservations")
            {
                // Reservation();
            }
        }
    }
}