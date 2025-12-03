static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();

    public static void Start()
    {
        MenuHelper menu = new MenuHelper(
            new[]{"Login", "Register"},
            "Do you want to login or register a new account?"
        );

        menu.Show();

        if (menu.SelectedIndex == 0)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the login page");
                Console.WriteLine("Please enter your email address");
                string email = Console.ReadLine();
                Console.WriteLine("Please enter your password");
                string password = Console.ReadLine();
                UserAccountModel acc = accountsLogic.CheckLogin(email, password);
                if (acc == null)
                {
                    Console.WriteLine("The email and password combination is not correct");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ReadLine();
                    Console.Clear();
                    Start();
                }

                if (acc == null)
                {
                    Console.WriteLine("Login failed");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ReadLine();
                }
                if (acc.IsAdmin == true)
                {
                    Console.WriteLine("Welcome back Admin " + acc.FullName);
                    Menu.AdminStart();
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
        else if (menu.SelectedIndex == 1)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the registration page");

                // Ask full name (no extra validation per request)
                Console.Write("Enter your full name: ");
                string fullName = Console.ReadLine();

                // Email loop
                string email;
                while (true)
                {
                    Console.Write("Enter your email address: ");
                    email = Console.ReadLine();

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
                    break;
                }

                // Password loop
                string password;
                while (true)
                {
                    Console.WriteLine("Enter your password: ");
                    Console.WriteLine("Password must be longer than 6 characters, Must contain an Uppercase and a Digit");
                    password = Console.ReadLine();

                    if (!accountsLogic.CheckPassword(password))
                    {
                        Console.WriteLine("Password does not meet the requirements. Try again.");
                        continue;
                    }
                    break;
                }

                // Date of birth loop
                string dateOfBirth;
                while (true)
                {
                    Console.Write("Enter your date of birth (DD-MM-YYYY): ");
                    dateOfBirth = Console.ReadLine();

                    if (!accountsLogic.CheckDob(dateOfBirth))
                    {
                        Console.WriteLine("Invalid date of birth format or not allowed. Try again.");
                        continue;
                    }
                    break;
                }


                UserAccountModel newAccount = accountsLogic.MakeAccount(email, password, fullName, dateOfBirth, false);
                

                if (newAccount != null)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Account successfully created! Please login with your new account");
                    Console.WriteLine("Press ENTER to continue");
                    Console.ReadLine();
                    Console.Clear();
                    Start();
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
