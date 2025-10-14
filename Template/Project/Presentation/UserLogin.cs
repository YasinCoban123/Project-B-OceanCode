static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();

    public static void Start()
    {
        Console.WriteLine();
        Console.WriteLine("Do you want to login or register a new account?");
        string accountchoice = Console.ReadLine();

        if (accountchoice.ToLower() == "login")
        {
            while (true)
            {
                Console.WriteLine();
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
                    Menu.Start();
                    break;
                }
            }
        }
        else if (accountchoice.ToLower() == "register")
        {
            Console.WriteLine();
            Console.WriteLine("Welcome to the registration page");

            while (true)
            {
                Console.Write("Enter your full name: ");
                string fullName = Console.ReadLine();

                Console.Write("Enter your email address: ");
                string email = Console.ReadLine();

                if (!accountsLogic.CheckEmailCorrect(email))
                {
                    Console.WriteLine("The email is not in the correct format. Try again.");
                    continue;
                }

                if (!accountsLogic.CheckIfEmailExist(email))
                {
                    Console.WriteLine("This email already exists. Please use another one.");
                    continue;
                }

                Console.Write("Enter your password: ");
                string password = Console.ReadLine();

                if (!accountsLogic.CheckPassword(password))
                {
                    Console.WriteLine("Password does not meet the requirements. Try again.");
                    continue;
                }

                Console.Write("Enter your date of birth (DD-MM-YYYY): ");
                string dateOfBirth = Console.ReadLine();

                if (!accountsLogic.CheckDob(dateOfBirth))
                {
                    Console.WriteLine("Invalid date of birth format or not allowed. Try again.");
                    continue;
                }

                // Als alles klopt -> maak account
                UserAccountModel newAccount = accountsLogic.MakeAccount(email, password, fullName, dateOfBirth);

                if (newAccount != null)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Account successfully created! Welcome, {newAccount.FullName}");
                    Menu.Start();
                    break;
                }
                else
                {
                    Console.WriteLine("Account could not be made, please check your input and try again.");
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
