static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {
        Console.WriteLine("Do you want to login or register a new account?");
        string accountchoice = Console.ReadLine();
        if (accountchoice.ToLower() == "login")
        {
            Console.WriteLine("Welcome to the login page");
            Console.WriteLine("Please enter your email address");
            string email = Console.ReadLine();
            Console.WriteLine("Please enter your password");
            string password = Console.ReadLine();
            UserAccountModel acc = accountsLogic.CheckLogin(email, password);
            if (acc != null)
            {
                Console.WriteLine("Welcome back " + acc.FullName);
                Console.WriteLine("Your email number is " + acc.EmailAddress);

                //Write some code to go back to the menu
                //Menu.Start();
            }
            else
            {
                Console.WriteLine("No account found with that email and password");
            }
        }
        else if (accountchoice.ToLower() == "register")
        {
            // Make an account method moet hier aangeroepen worden
        }
        else
        {
            Console.WriteLine("Invalid choice, please type 'login' or 'register'");
            Start();
        }
        
    }
}