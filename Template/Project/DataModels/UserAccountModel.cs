public class UserAccountModel
{

    public Int64 Id { get; set; }
    public string EmailAddress { get; set; }

    public string Password { get; set; }

    public string FullName { get; set; }

    public string DateOfBirth { get; set; }

    public UserAccountModel(string email, string password, string fullname, string dateofbirth)
    {
        EmailAddress = email;
        Password = password;
        FullName = fullname;
        DateOfBirth = dateofbirth;
    }


}



