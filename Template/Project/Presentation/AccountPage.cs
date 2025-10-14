using System;

public static class AccountPage
{
    public static void Start(UserAccountModel user)
    {
        UserAccountsAccess acces = new();
        Console.WriteLine();
        Console.WriteLine("[1] See info");
        Console.WriteLine("[2] Edit Account");
        Console.WriteLine("[3] Delete Account");
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
            Menu.Start();
        }
        else if (choice == "2")
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
            Menu.Start();
        }
        else if (choice == "3")
        {
            acces.Delete(user);
            Console.WriteLine("Account deleted successfully!");
            UserLogin.Start();
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }
}
