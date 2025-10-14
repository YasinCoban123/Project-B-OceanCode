static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start(string email)
    {
        Console.WriteLine();
        Console.WriteLine("[1] Screenings");
        Console.WriteLine("[2] View reservations");
        Console.WriteLine("[3] Account");

        string input = Console.ReadLine();
        if (input == "1")
        {
            Console.WriteLine("This feature is not yet implemented");
            Start(email);
        }
        else if (input == "2")
        {
            Console.WriteLine("This feature is not yet implemented");
            Start(email);
        }
        else if (input == "3")
        {
            UserAccountsAccess acces = new();
            UserAccountModel currentUser = acces.GetByEmail(email);
            AccountPage.Start(currentUser, new UserAccountsAccess());
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start(email);
        }

    }
}