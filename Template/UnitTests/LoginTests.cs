namespace UnitTests
{
    [TestClass]
    public class LoginTests
    {
        private AccountsLogic accountsLogic;

        [TestInitialize]
        public void Setup()
        {
            accountsLogic = new AccountsLogic();
        }

        // Happy Path: Successful user login
        [DataTestMethod]
        [DataRow("user@gmail.com", "User123", "Normal User")]
        public void Login_With_Valid_User_Credentials(
            string email,
            string password,
            string fullName)
        {
            // Arrange: Create the account
            var account = accountsLogic.MakeAccount(email, password, fullName, "01-01-2000", false);
            Assert.IsNotNull(account);

            // Act: Check login
            var loggedInUser = accountsLogic.CheckLogin(email, password);

            // Assert: Login succeeds
            Assert.IsNotNull(loggedInUser);
            Assert.AreEqual(email, loggedInUser.Email);
            Assert.IsFalse(loggedInUser.IsAdmin);

            // Cleanup
            accountsLogic.DeleteAccount(email);
        }

        // Happy Path: Successful admin login
        [DataTestMethod]
        [DataRow("admin@gmail.com", "Admin123", "Admin User")]
        public void Login_With_Valid_Admin_Credentials(
            string email,
            string password,
            string fullName)
        {
            var account = accountsLogic.MakeAccount(email, password, fullName, "01-01-1990", true);
            Assert.IsNotNull(account);

            var loggedInAdmin = accountsLogic.CheckLogin(email, password);

            Assert.IsNotNull(loggedInAdmin);
            Assert.AreEqual(email, loggedInAdmin.Email);
            Assert.IsTrue(loggedInAdmin.IsAdmin);

            accountsLogic.DeleteAccount(email);
        }

        // Sad Path: Invalid credentials
        [DataTestMethod]
        [DataRow("user@gmail.com", "User123", "WrongPassword")]
        [DataRow("nonexistent@gmail.com", "Password123", "Password123")]
        public void Login_With_Invalid_Credentials_Fails(
            string email,
            string correctPassword,
            string wrongPassword)
        {
            // Arrange: Create account if it exists
            if (!email.Contains("nonexistent"))
            {
                var account = accountsLogic.MakeAccount(email, correctPassword, "Test User", "01-01-2000", false);
                Assert.IsNotNull(account);
            }

            // Act: Try logging in with wrong credentials
            var loggedInUser = accountsLogic.CheckLogin(email, wrongPassword);

            // Assert: Login fails
            Assert.IsNull(loggedInUser);

            // Cleanup
            if (!email.Contains("nonexistent"))
            {
                accountsLogic.DeleteAccount(email);
            }
        }

        // Sad Path: Empty email or password
        [DataTestMethod]
        [DataRow("", "Password123")]
        [DataRow("user@gmail.com", "")]
        [DataRow("", "")]
        public void Login_With_Empty_Or_Missing_Input_Fails(
            string email,
            string password)
        {
            // Arrange: Create account
            var account = accountsLogic.MakeAccount("user@gmail.com", "Password123", "Test User", "01-01-2000", false);
            Assert.IsNotNull(account);

            // Act: Try logging in with empty input
            var loggedInUser = accountsLogic.CheckLogin(email, password);

            // Assert: Login fails
            Assert.IsNull(loggedInUser);

            // Cleanup
            accountsLogic.DeleteAccount("user@gmail.com");
        }
    }
}