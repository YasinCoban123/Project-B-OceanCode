using System;

public static class AccountPage
{
    public static void Start(UserAccountModel user)
    {
        UserAccountsAccess acces = new();
        AccountsLogic accountsLogic = new AccountsLogic();
        Console.WriteLine();
        Console.WriteLine("[1] See info");
        Console.WriteLine("[2] Edit Account");
        Console.WriteLine("[3] Delete Account");
        Console.WriteLine("[4] Go Back");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            Console.WriteLine();
            Console.WriteLine($"ID: {user.AccountId}");
            Console.WriteLine($"Name: {user.FullName}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Password: {user.Password}");
            Console.WriteLine($"Date of Birth: {user.DateOfBirth}");
            Start(user);
        }
        else if (choice == "2")
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
            Start(user);
        }

        else if (choice == "3")
        {

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
        else if (choice == "4")
        {
            Menu.Start();
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }
}
