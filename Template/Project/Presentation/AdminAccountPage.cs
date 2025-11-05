using System;

public static class AdminAccountPage
{
    public static void Start(UserAccountModel user)
    {
        UserAccountsAccess acces = new();
        AccountsLogic logic = new();
        Console.WriteLine();
        Console.WriteLine("[1] See own Account");
        Console.WriteLine("[2] See users Account");
        Console.WriteLine("[3] Go Back");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            Console.WriteLine();
            Console.WriteLine("[1] See info");
            Console.WriteLine("[2] Edit Account");
            Console.WriteLine("[3] Delete Account");
            Console.WriteLine("[4] Go Back");
            Console.Write("Choose an option: ");
            string accountchoice = Console.ReadLine();
            Menu.AdminStart();

            if (accountchoice == "1")
            {
                Console.WriteLine();
                Console.WriteLine($"ID: {user.AccountId}");
                Console.WriteLine($"Name: {user.FullName}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Password: {user.Password}");
                Console.WriteLine($"Date of Birth: {user.DateOfBirth}");
                Menu.AdminStart();
            }
            else if (accountchoice == "2")
            {
                Console.Write("Enter new full name (leave blank to keep current): ");
                string newName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName))
                    user.FullName = newName;

                Console.Write("Enter new email (leave blank to keep current): ");
                string newEmail = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newEmail))
                    user.Email = newEmail;

                Console.Write("Enter new password (leave blank to keep current): ");
                string newPassword = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newPassword))
                    user.Password = newPassword;

                Console.Write("Enter new date of birth (leave blank to keep current): ");
                string newDob = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newDob))
                    user.DateOfBirth = newDob;

                acces.Update(user);
                Console.WriteLine("Account updated successfully!");
                Start(user);
            }
            else if (accountchoice == "3")
            {
                acces.Delete(user);
                Console.WriteLine("Account deleted successfully!");
                UserLogin.Start();
            }

            else if (accountchoice == "4")
            {
                Menu.AdminStart();
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
    
        }

        if (choice == "2")
        {
            Console.WriteLine();
            Console.WriteLine("[1] See all users info");
            Console.WriteLine("[2] Edit user Account");
            Console.WriteLine("[3] Delete user Account");
            Console.WriteLine("[4] Go Back");
            Console.Write("Choose an option: ");
            string accountchoice = Console.ReadLine();

            if (accountchoice == "1")
            {
                List<UserAccountModel> AllUserAccounts = logic.GetAllUserAccounts();
                foreach (UserAccountModel account in AllUserAccounts)
                {
                    Console.WriteLine();
                    Console.WriteLine($"ID: {account.AccountId}");
                    Console.WriteLine($"Name: {account.FullName}");
                    Console.WriteLine($"Email: {account.Email}");
                    Console.WriteLine($"Password: {account.Password}");
                    Console.WriteLine($"Date of Birth: {account.DateOfBirth}");
                }
                Start(user);
            }

            else if (accountchoice == "2")
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

                    Console.Write("Enter new email (leave blank to keep current): ");
                    string newEmail = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newEmail))
                        chosenuser.Email = newEmail;

                    Console.Write("Enter new password (leave blank to keep current): ");
                    string newPassword = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newPassword))
                        chosenuser.Password = newPassword;

                    Console.Write("Enter new date of birth (leave blank to keep current): ");
                    string newDob = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newDob))
                        chosenuser.DateOfBirth = newDob;

                    acces.Update(chosenuser);
                    Console.WriteLine("Account updated successfully!");
                    Start(user);
                }
            }

            else if (accountchoice == "3")
            {
                Console.WriteLine("Give the ID of the account you want to delete");
                string chosenID = Console.ReadLine();
                int ChosenID = Convert.ToInt32(chosenID);
                UserAccountModel chosenuser = logic.GetUserAccount(ChosenID);
                if (chosenuser == null)
                {
                    Console.WriteLine("Admin account cannot be deleted!");
                }
                else
                {
                  acces.Delete(chosenuser);
                    Console.WriteLine("Account deleted successfully!");
                    Start(user);  
                }
                
            }
            
            else if (accountchoice == "4")
            {
                Start(user);
            }
        }

        if (choice == "3")
        {
            Menu.AdminStart();
        }

    }
}
