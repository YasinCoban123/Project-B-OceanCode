using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class RegisterTests
{
    private AccountsLogic accountsLogic;

    [TestInitialize]
    public void Setup()
    {
        accountsLogic = new AccountsLogic();
    }

    [DataTestMethod]
    [DataRow("naam@gmail.com", "Naam123", "naam", "01-01-2000")]
    public void RegisterScreen_Creates_Account(
        string email,
        string password,
        string fullName,
        string dob)
    {
        Assert.IsTrue(accountsLogic.CheckEmailCorrect(email));
        Assert.IsTrue(accountsLogic.CheckPassword(password));
        Assert.IsTrue(accountsLogic.CheckDob(dob));

        var account = accountsLogic.MakeAccount(
            email,
            password,
            fullName,
            dob,
            false
        );

        Assert.IsNotNull(account);

        accountsLogic.DeleteAccount(email);
    }

    [DataTestMethod]
    [DataRow("naam@gmail.com", "naam2@gmail.com", "naam")]
    public void Two_Accounts_With_Same_Name_Are_Created(
        string email1,
        string email2,
        string fullName)
    {
        var acc1 = accountsLogic.MakeAccount(
            email1,
            "Password1",
            fullName,
            "01-01-2000",
            false
        );

        var acc2 = accountsLogic.MakeAccount(
            email2,
            "Password1",
            fullName,
            "02-02-2000",
            false
        );

        Assert.IsNotNull(acc1);
        Assert.IsNotNull(acc2);
        Assert.AreEqual(acc1.FullName, acc2.FullName);

        accountsLogic.DeleteAccount(email1);
        accountsLogic.DeleteAccount(email2);
    }

    [DataTestMethod]
    [DataRow("naam@gmail.com")]
    public void Two_Accounts_With_Same_Email_And_Password_Are_Not_Created(
        string email)
    {
        var acc1 = accountsLogic.MakeAccount(
            email,
            "Naam123",
            "naam1",
            "01-01-2000",
            false
        );

        var acc2 = accountsLogic.MakeAccount(
            email,
            "Naam123",
            "naam2",
            "01-01-2000",
            false
        );

        Assert.IsNotNull(acc1);
        Assert.IsNull(acc2);

        accountsLogic.DeleteAccount(email);
    }

    [DataTestMethod]
    [DataRow("naam@gmail.com", "naam2@gmail.com", "Naam")]
    public void Two_Accounts_With_Same_Name_Still_Exist(
        string email1,
        string email2,
        string fullName)
    {
        var acc1 = accountsLogic.MakeAccount(
            email1,
            "Password1",
            fullName,
            "01-01-2000",
            false
        );

        var acc2 = accountsLogic.MakeAccount(
            email2,
            "Password1",
            fullName,
            "01-01-2000",
            false
        );

        Assert.IsNotNull(acc1);
        Assert.IsNotNull(acc2);

        accountsLogic.DeleteAccount(email1);
        accountsLogic.DeleteAccount(email2);
    }
}
