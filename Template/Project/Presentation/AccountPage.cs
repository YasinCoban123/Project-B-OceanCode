using System;

public static class AccountPage
{
    public static void Start(UserAccountModel user)
    {
        UserAccountsAccess acces = new();
        AccountsLogic accountsLogic = new();

        string[] options = { "See info", "Edit Account", "Delete Account", "Go Back" };
        int selected = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Account Menu (use Arrow keys and Enter to select, Esc to go back)\n");

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {options[i]}");
                }
            }

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.DownArrow)
            {
                selected = (selected + 1) % options.Length;
                continue;
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                selected = (selected - 1 + options.Length) % options.Length;
                continue;
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                Menu.Start();
                return;
            }
            else if (key.Key != ConsoleKey.Enter)
            {
                continue;
            }

            // Enter pressed -> act on selection
            switch (selected)
            {
                case 0: // See info
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine($"ID: {user.AccountId}");
                    Console.WriteLine($"Name: {user.FullName}");
                    Console.WriteLine($"Email: {user.Email}");
                    Console.WriteLine($"Password: {user.Password}");
                    Console.WriteLine($"Date of Birth: {user.DateOfBirth}");
                    Console.WriteLine("\nPress any key to return to menu...");
                    Console.ReadKey(true);
                    break;

                case 1: // Edit Account
                    Console.Clear();
                    Console.Write("Enter new full name (leave blank to keep current): ");
                    string newName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newName))
                        user.FullName = newName;

                    while (true)
                    {
                        Console.Write("Enter new email (leave blank to keep current): ");
                        string newEmail = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newEmail))
                        {
                            break;
                        }

                        if (!accountsLogic.CheckEmailCorrect(newEmail))
                        {
                            Console.WriteLine("The email is not in the correct format. Try again.");
                            continue;
                        }

                        if (!accountsLogic.CheckIfEmailExist(newEmail))
                        {
                            Console.WriteLine("This email already exists. Please use another one.");
                            continue;
                        }

                        user.Email = newEmail;
                        break;
                    }

                    while (true)
                    {
                        Console.WriteLine("Password must be longer than 6 characters, Must contain an Uppercase and a Digit");
                        Console.Write("Enter new password (leave blank to keep current): ");
                        string newPassword = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(newPassword))
                        {
                            break;
                        }

                        if (!accountsLogic.CheckPassword(newPassword))
                        {
                            Console.WriteLine("Password does not meet the requirements. Try again.");
                            continue;
                        }

                        user.Password = newPassword;
                        break;
                    }

                    while (true)
                    {
                        Console.Write("Enter new date of birth (leave blank to keep current): ");
                        string newDob = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(newDob))
                        {
                            break;
                        }

                        if (!accountsLogic.CheckDob(newDob))
                        {
                            Console.WriteLine("Invalid date of birth format or not allowed. Try again.");
                            continue;
                        }

                        user.DateOfBirth = newDob;
                        break;
                    }

                    acces.Update(user);
                    Console.WriteLine("Account updated successfully!");
                    Console.WriteLine("\nPress any key to return to menu");
                    Console.ReadKey(true);
                    break;

                case 2: // Delete Account
                    Console.Clear();
                    Console.Write("Are you sure you want to delete the account? (y/n): ");
                    var confirmKey = Console.ReadKey(true);
                    if (confirmKey.Key == ConsoleKey.Y)
                    {
                        if (acces.Delete(user) == false)
                        {
                            Console.WriteLine("Account cannot be deleted, remove reservation on the account first");
                            Console.WriteLine("\nPress any key to return to menu");
                            Console.ReadKey(true);
                            Menu.Start();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Account deleted successfully!");
                            Console.WriteLine("\nPress any key to continue");
                            Console.ReadKey(true);
                            UserLogin.Start();
                            return;
                        }
                    }
                    // if not confirmed, return to menu
                    break;

                case 3: // Go Back
                    Menu.Start();
                    return;
            }
        }
    }
}
