

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
        UserAccountModel accountinfo = access.GetByEmail(email);

        if (password == accountinfo.Password)
        {
            UserAccountModel acc = _access.GetByEmail(email);
            if (acc != null)
            {
                CurrentAccount = acc;
                return acc;
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
        UserAccountsAccess access = new UserAccountsAccess();
        UserAccountModel accountinfo = access.GetByEmail(email);

        if (accountinfo is null)
        {
            return true;
        }
        
        if (email == accountinfo.Email)
        {
            return false;
        }
        return true;
    }

}
