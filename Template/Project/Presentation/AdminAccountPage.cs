using System;

public static class AdminAccountPage
{
    public static void Start(UserAccountModel user)
    {
        UserAccountsAccess acces = new();
        AccountsLogic logic = new();

        MenuHelper menu = new MenuHelper(new[]{
            "See own Account",
            "See users Account",
            "Create Admin account",
            "Go Back"
        },
        "Admin Options");

        menu.Show();
        int choiceIndex = menu.SelectedIndex;

        if (choiceIndex == 0)
        {
            MenuHelper ownMenu = new MenuHelper(new[]{
                "See info",
                "Edit Account",
                "Delete Account",
                "Go Back"
            },
            "Account Options");

            ownMenu.Show();
            int accountchoice = ownMenu.SelectedIndex;

            if (accountchoice == 0)
            {
                Console.WriteLine();
                Console.WriteLine($"ID: {user.AccountId}");
                Console.WriteLine($"Name: {user.FullName}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Password: {user.Password}");
                Console.WriteLine($"Date of Birth: {user.DateOfBirth}");
                Console.WriteLine(" ");
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
                Console.Clear();
                Menu.AdminStart();
            }
            else if (accountchoice == 1)
            {
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

                    if (logic.CheckEmailCorrect(newEmail) == false)
                    {
                        Console.WriteLine("The email is not in the correct format. Try again.");
                        continue;
                    }

                    if (logic.CheckIfEmailExist(newEmail)== false)
                    {
                        Console.WriteLine("This email already exists. Please use another one.");
                        continue;
                    }

                    user.Email = newEmail;
                    break;
                }

                while (true)
                {
                    Console.Write("Enter new password (leave blank to keep current): ");
                    string newPassword = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(newPassword))
                    {
                        break;
                    }

                    if (!logic.CheckPassword(newPassword))
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

                    if (!logic.CheckDob(newDob))
                    {
                        Console.WriteLine("Invalid date of birth format or not allowed. Try again.");
                        continue;
                    }

                    user.DateOfBirth = newDob;
                    break;
                }

                acces.Update(user);
                Console.WriteLine("Account updated successfully!");
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
                Console.Clear();
                Start(user);
            }
            else if (accountchoice == 2)
            {
                acces.Delete(user);
                Console.WriteLine("Account deleted successfully!");
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
                Console.Clear();
                UserLogin.Start();
            }
            else if (accountchoice == 3)
            {
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
                Console.Clear();
                Menu.AdminStart();
            }
        }

        if (choiceIndex == 1)
        {
            MenuHelper userMenu = new MenuHelper(new[]{
                "See all users info",
                "Edit user Account",
                "Delete user Account",
                "Go Back"
            },
            "User Management");

            userMenu.Show();
            int accountchoice = userMenu.SelectedIndex;

            if (accountchoice == 0)
            {
                List<UserAccountModel> AllUserAccounts = logic.GetAllUserAccounts();
                foreach (UserAccountModel account in AllUserAccounts)
                {
                    Console.WriteLine();
                    Console.WriteLine($"ID: {account.AccountId}");
                    Console.WriteLine($"Name: {account.FullName}");
                    Console.WriteLine($"Email: {account.Email}");
                    Console.WriteLine($"Date of Birth: {account.DateOfBirth}");
                }
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
                Console.Clear();
                Start(user);
            }
            else if (accountchoice == 1)
            {
                Console.WriteLine("Give the ID of the account you want to edit");
                string chosenID = Console.ReadLine();
                int ChosenID = Convert.ToInt32(chosenID);
                UserAccountModel chosenuser = logic.GetUserAccount(ChosenID);
                if (chosenuser == null)
                {
                    Console.WriteLine("Admin account cannot be edited!");
                }
                else
                {
                    Console.Write("Enter new full name (leave blank to keep current): ");
                    string newName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newName))
                        chosenuser.FullName = newName;

                    while (true)
                    {
                        Console.Write("Enter new email (leave blank to keep current): ");
                        string newEmail = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newEmail))
                        {
                            break;
                        }

                        if (!logic.CheckEmailCorrect(newEmail))
                        {
                            Console.WriteLine("The email is not in the correct format. Try again.");
                            continue;
                        }

                        if (!logic.CheckIfEmailExist(newEmail))
                        {
                            Console.WriteLine("This email already exists. Please use another one.");
                            continue;
                        }

                        chosenuser.Email = newEmail;
                        break;
                    }

                    while (true)
                    {
                        Console.Write("Enter new password (leave blank to keep current): ");
                        string newPassword = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(newPassword))
                        {
                            break;
                        }

                        if (!logic.CheckPassword(newPassword))
                        {
                            Console.WriteLine("Password does not meet the requirements. Try again.");
                            continue;
                        }

                        chosenuser.Password = newPassword;
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

                        if (!logic.CheckDob(newDob))
                        {
                            Console.WriteLine("Invalid date of birth format or not allowed. Try again.");
                            continue;
                        }

                        chosenuser.DateOfBirth = newDob;
                        break;
                    }

                    acces.Update(chosenuser);
                    Console.WriteLine("Account updated successfully!");
                    Console.WriteLine("Press ENTER to continue");
                    Console.ReadLine();
                    Console.Clear();
                    Start(user);
                }
            }
            else if (accountchoice == 2)
            {
                Console.WriteLine("Give the ID of the account you want to delete");
                string chosenID = Console.ReadLine();
                int ChosenID = Convert.ToInt32(chosenID);
                UserAccountModel chosenuser = logic.GetUserAccount(ChosenID);
                if (chosenuser == null)
                {
                    Console.WriteLine("Admin account cannot be deleted!");
                }
                if (acces.Delete(chosenuser) == false)
                {
                    Console.WriteLine("Account cannot be deleted, User must remove reservation on the account first");
                    Console.WriteLine("Press ENTER to continue");
                    Console.ReadLine();
                    Console.Clear();
                    Start(user);
                }
                else
                {
                    acces.Delete(chosenuser);
                    Console.WriteLine("Account deleted successfully!");
                    Console.Clear();
                    Start(user);
                }
            }
            else if (accountchoice == 3)
            {
                Console.Clear();
                Start(user);
            }
        }

        if (choiceIndex == 2)
        {
            Console.WriteLine();
            Console.Write("Name: ");
            string fullName = Console.ReadLine();

            string email;
            while (true)
            {
                Console.Write("Email adress: ");
                email = Console.ReadLine();

                if (!logic.CheckEmailCorrect(email))
                {
                    Console.WriteLine("The email is not in the correct format. Try again.");
                    continue;
                }

                if (!logic.CheckIfEmailExist(email))
                {
                    Console.WriteLine("This email already exists. Please use another one.");
                    continue;
                }
                break;
            }

            string password;
            while (true)
            {
                Console.Write("Password: ");
                password = Console.ReadLine();

                if (!logic.CheckPassword(password))
                {
                    Console.WriteLine("Password does not meet the requirements. Try again.");
                    continue;
                }
                break;
            }

            string dateOfBirth;
            while (true)
            {
                Console.Write("Date of birth: ");
                dateOfBirth = Console.ReadLine();

                if (!logic.CheckDob(dateOfBirth))
                {
                    Console.WriteLine("Invalid date of birth format or not allowed. Try again.");
                    continue;
                }
                break;
            }

            UserAccountModel newAccount = logic.MakeAdminAccount(email, password, fullName, dateOfBirth);

            if (newAccount != null)
            {
                Console.WriteLine();
                Console.WriteLine($"Admin account successfully created!");
            }
            Console.WriteLine($"Press Enter to continue");
            Console.Clear();
            Menu.AdminStart();
        }

        if (choiceIndex == 3)
        {
            Console.Clear();
            Menu.AdminStart();
        }
    }
}
