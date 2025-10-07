

//This class is not static so later on we can use inheritance and interfaces
public class AccountsLogic
{

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    public static UserAccountModel? CurrentAccount { get; private set; }
    private AccountsAccess _access = new();

    public AccountsLogic()
    {
        // Could do something here

    }

    public UserAccountModel CheckLogin(string email, string password)
{
    if (!CheckPassword(password)) // method to be made by Arjun
    {
        return null;
    }

    // haalt dictionary op van de database
    Dictionary<string, string> accounts = EmailPasswordDict(); // method to be made by Amine

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
}




