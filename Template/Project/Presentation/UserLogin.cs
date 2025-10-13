static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {
        Console.WriteLine("Do you want to login or register a new account?");
        string accountchoice = Console.ReadLine();

        if (accountchoice.ToLower() == "login")
        {
            while (true)
            {
                Console.WriteLine("Welcome to the login page");
                Console.WriteLine("Please enter your email address");
                string email = Console.ReadLine();
                Console.WriteLine("Please enter your password");
                string password = Console.ReadLine();
                UserAccountModel acc = accountsLogic.CheckLogin(email, password);

                if (acc == null)
                {
                    Console.WriteLine("Login failed");
                }
                else
                {
                    Console.WriteLine("Welcome back " + acc.FullName);
                    Console.WriteLine("Your email number is " + acc.Email);

                    //Write some code to go back to the menu
                    //Menu.Start();
                    break;
                }             
            }
        }

        else if (accountchoice.ToLower() == "register")
        {
            Console.WriteLine("Welcome to the registration page");

            while (true) 
            {
                Console.Write("Enter your full name: ");
                string fullName = Console.ReadLine();

                Console.Write("Enter your email address: ");
                string email = Console.ReadLine();

                Console.Write("Enter your password: ");
                string password = Console.ReadLine();

                Console.Write("Enter your date of birth (DD-MM-YYYY): ");
                string dobString = Console.ReadLine();

                // roept de make account method aan in de logic file
                UserAccountModel newAccount = accountsLogic.MakeAccount(email, password, fullName, dobString);

                if (newAccount != null)
                {
                    Console.WriteLine($"Account successfully created! Welcome, {newAccount.FullName}");
                    // Menu word aangezet nadat alle files compleet zijn ervoor
                    // Menu.Start();
                    break; // exit de loop zodra het account is aangemaakt
                }
                else
                {
                    Console.WriteLine("Account could not be made, please check your input and try again");
                }
            }
        }


        else
        {
            Console.WriteLine("Invalid choice, please type 'login' or 'register'");
            Start();
        }
        
    }
}