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
                    TryApply = (u, input) =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return (true, null);
        
                        u.FullName = input;
                        return (true, null);
                    }
                },
        
                new EditOption<UserAccountModel>
                {
                    Label = "Email",
                    Display = u => u.Email,
                    TryApply = (u, input) =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return (true, null);
        
                        if (!logic.CheckEmailCorrect(input))
                            return (false, "Email format is invalid.");
        
                        if (!logic.CheckIfEmailExist(input))
                            return (false, "This email already exists.");
        
                        u.Email = input;
                        return (true, null);
                    }
                },
        
                new EditOption<UserAccountModel>
                {
                    Label = "Password",
                    Display = u => new string('*', u.Password?.Length ?? 0),
                    TryApply = (u, input) =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return (true, null);
        
                        if (!logic.CheckPassword(input))
                            return (false, "Password does not meet requirements.");
        
                        u.Password = input;
                        return (true, null);
                    }
                },
        
                new EditOption<UserAccountModel>
                {
                    Label = "Date of Birth",
                    Display = u => u.DateOfBirth,
                    TryApply = (u, input) =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return (true, null);
        
                        if (!logic.CheckDob(input))
                            return (false, "Invalid date of birth format.");
        
                        u.DateOfBirth = input;
                        return (true, null);
                    }
                }
            }
        );

        return accountEditor.Start();
    }
    
}
