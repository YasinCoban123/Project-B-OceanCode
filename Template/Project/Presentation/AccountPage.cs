using System;

public static class AccountPage
{
    public static void Start(UserAccountModel user)
    {
        UserAccountsAccess acces = new();
        AccountsLogic accountsLogic = new AccountsLogic();

        MenuHelper menu = new MenuHelper(new[]{
            "See info",
            "Edit Account",
            "Delete Account",
            "Go Back"
        },
        "Account Menu");

        menu.Show();

        int choice = menu.SelectedIndex;
        Console.Clear();

        if (choice == 0)
        {
            Console.WriteLine();
            Console.WriteLine($"ID: {user.AccountId}");
            Console.WriteLine($"Name: {user.FullName}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Date of Birth: {user.DateOfBirth}");

            Console.WriteLine();
            Console.WriteLine("Press ENTER to go back.");
            Console.ReadLine();
            Start(user);
        }
        else if (choice == 1)
        {
            user = EditUserScreen(user);
            acces.Update(user);
            Console.WriteLine("Account updated successfully!");
            Start(user);
        }

        else if (choice == 2)
        {
            // Confirm deletion
            Console.WriteLine("Are you sure you want to delete your account? This action cannot be undone. (yes/no)");
            string confirmation = Console.ReadLine().Trim().ToLower();
            if (confirmation != "yes")
            {
                Console.WriteLine("Account deletion cancelled.");
                Menu.Start();
                return;
            }

            if (acces.Delete(user) == false)
            {
                Console.WriteLine("Account cannot be deleted, remove reservation on the account first");
                Menu.Start();
            }
            else
            {
                Console.WriteLine("Account deleted successfully!");
                UserLogin.Start();
            }
        }
        else if (choice == 3)
        {
            Menu.Start();
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }
    
    public static UserAccountModel EditUserScreen(UserAccountModel user)
    {
        AccountsLogic logic = new();
        var accountEditor = new ItemEditor<UserAccountModel>(
            user,
            "Edit account screen",
            new List<EditOption<UserAccountModel>>
            {
                new EditOption<UserAccountModel>
                {
                    Label = "Full Name",
                    Display = u => u.FullName,
                    OnSelect = u =>
                    {
                        Console.Write("Enter new full name (leave blank to keep current): ");
                        string newName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newName))
                            u.FullName = newName;
                    }
                },
                new EditOption<UserAccountModel>
                {
                    Label = "Email",
                    Display = u => u.Email,
                    OnSelect = u =>
                    {
                        while (true)
                        {
                            Console.Write("Enter new email (leave blank to keep current): ");
                            string newEmail = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(newEmail))
                                break;
                            if (!logic.CheckEmailCorrect(newEmail))
                            {
                                Console.WriteLine("The email is not in the correct format. Try again.");
                                continue;
                            }
                            if (!logic.CheckIfEmailExist(newEmail))
                            {
                                Console.WriteLine("This email already exists. Choose another.");
                                continue;
                            }
                            u.Email = newEmail;
                            break;
                        }
                    }
                },
                new EditOption<UserAccountModel>
                {
                    Label = "Password",
                    Display = u => new string('*', u.Password?.Length ?? 0),
                    OnSelect = u =>
                    {
                        while (true)
                        {
                            Console.Write("Enter new password (leave blank to keep current): ");
                            string newPassword = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(newPassword))
                                break;
                            if (!logic.CheckPassword(newPassword))
                            {
                                Console.WriteLine("Password does not meet requirements.");
                                continue;
                            }
                            u.Password = newPassword;
                            break;
                        }
                    }
                },
                new EditOption<UserAccountModel>
                {
                    Label = "Date of Birth",
                    Display = u => u.DateOfBirth,
                    OnSelect = u =>
                    {
                        while (true)
                        {
                            Console.Write("Enter new date of birth (leave blank to keep current): ");
                            string newDob = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(newDob))
                                break;
                            if (!logic.CheckDob(newDob))
                            {
                                Console.WriteLine("Invalid format or not allowed.");
                                continue;
                            }
                            u.DateOfBirth = newDob;
                            break;
                        }
                    }
                }
            }
        );
        return accountEditor.Start();
    }
    
}
