public class UserAccountModel
{
    public long AccountId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string DateOfBirth { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }


    public UserAccountModel() { }  // empty overload for dapper because constructor below doesnt contain accoundId which dapper needs to create object

    public UserAccountModel(string fullName, string email, string dateOfBirth, string password, bool isadmin)
    {
        FullName = fullName;
        Email = email;
        DateOfBirth = dateOfBirth;
        Password = password;
        IsAdmin = isadmin;
    }
    
}
