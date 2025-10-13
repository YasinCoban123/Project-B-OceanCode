

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
        UserAccountsAccess access = new UserAccountsAccess();
        Dictionary<string, string> accounts = access.EmailPasswordDict(); // method to be made by Amine
        List<string> emails = access.GetAllEmails();

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


    public UserAccountModel MakeAccount(string email, string password, string fullName, string dateOfBirth)
    {
        if (!CheckEmailCorrect(email))
        {
            return null;
        }
        if (!CheckPassword(password))
        {
            return null;
        }

        if (!CheckDob(dateOfBirth))
        {
            return null;
        }

        if (!CheckIfEmailExist(email))
        {
            return null;
        }
        UserAccountModel newAccount = new UserAccountModel(fullName, email, dateOfBirth, password);
        _access.Write(newAccount);

        CurrentAccount = newAccount;
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
        if (!password.Any(ch => char.IsDigit(ch)))
        {
            return false;
        }
        return true;
    }

    private bool CheckEmailCorrect(string email)
    {
        return email.Contains("@");
    }

    private bool CheckDob(string dobString)
    {
        return dobString.Length == 10 && dobString[2] == '-' && dobString[5] == '-';
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
