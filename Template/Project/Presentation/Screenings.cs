static class Screenings
{
    static private ScreeningLogic screeningLogic = new ScreeningLogic();
    public static void Start()
    {
        UserAccountModel ?currentUser = AccountsLogic.CurrentAccount;

        Console.WriteLine($"Welcome to the screenings page, {currentUser.FullName}");
        List<string> screenings = screeningLogic.ShowScreenings();

        foreach (string screening in screenings)
        {
            Console.WriteLine(screening);
        }
        Console.Write("Enter the ScreeningId you want to reserve: ");
        string input = Console.ReadLine();
        int screeningId = Convert.ToInt32(input);


        // Logic layer handelt validatie en reservering af
        bool success = screeningLogic.MakeReservation(currentUser.AccountId, screeningId);

        if (success)
        {
            Console.WriteLine("Reservation successful!");
            Menu.Start();
        }
        else
        {
            Console.WriteLine("Failed to make reservation.");
        }
    }
}