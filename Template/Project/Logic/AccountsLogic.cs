

//This class is not static so later on we can use inheritance and interfaces
public class AccountsLogic
{

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    public static UserAccountModel? CurrentAccount { get; private set; }
    private UserAccountsAccess _access = new();

    public AccountsLogic()
    {
        // Could do something here

    }

    public UserAccountModel CheckLogin(string email, string password)
    {
        if (!CheckEmailCorrect(email))
        {
            return null;
        }
        if (!CheckPassword(password)) // method to be made by Arjun
        {
            return null;
        }

        UserAccountsAccess access = new UserAccountsAccess();
        Dictionary<string, string> accounts = access.EmailPasswordDict(); // method to be made by Amine

        // loopt door accounts
        foreach (var account in accounts)
        {
            string storedEmail = account.Key;
            string storedPassword = account.Value;

            if (email.ToLower() == storedEmail.ToLower() && password == storedPassword)
            {
                UserAccountModel acc = _access.GetByEmail(email);
                if (acc != null)
                {
                    CurrentAccount = acc;
                    return acc;
                }
            }
        }
        return null;
    }

    public UserAccountModel MakeAccount(
    string email,
    string password,
    string fullName,
    string dobString)
    {
        if (!CheckEmailCorrect(email))
        {
            return null;
        }
    
        if (!CheckPassword(password))
        {
            return null;
        }
    
        // checkt of de dob in format DD-MM-YYYY
        if (!CheckDob(dobString))
        {
            return null;
        }
    
        // check of de email al bestaat in de database
        if (CheckIfEmailExist(email))
        {
            return null;
        }
    
        // na alle checks, maak de object aan met de info van de user
        UserAccountModel newAccount = new UserAccountModel(email, password, fullName, dobString);

        _access.Write(newAccount); // voeg de object aan de DB toe

        CurrentAccount = newAccount; // assignt de CurrentAccount als de nieuwe account, dus automatisch ingelogt
        return newAccount;
    }


    private bool CheckPassword(string password)
    {
        if (password.Length < 6)
        {
            return false;
        }
        if (!password.Any(ch => char.IsUpper(ch)))
        {
            return false;
        }
        if (password.Any(ch => !char.IsLetterOrDigit(ch)))
        {
            return false;
        }
        return true;
    }

    private bool CheckEmailCorrect(string email)
    {
        return email.Contains("@");
    }

    private bool CheckIfEmailExist(string email)
    {
        List<string> emails = _access.GetAllEmails();
        foreach (string existingEmail in emails)
        {
            if (email.ToLower() == existingEmail.ToLower())
            {
                return false;
            }
        }
        return true;
    }
}




