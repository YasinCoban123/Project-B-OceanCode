using Microsoft.Data.Sqlite;

using Dapper;


public class UserAccountsAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private string Table = "UserAccount";

    public void Write(UserAccountModel account)
    {
        string sql = $"INSERT INTO {Table} (email, password, fullname, dateofbirth) VALUES (@Email, @Password, @FullName, @DateOfBirth)";
        _connection.Execute(sql, account);
    }

    public UserAccountModel GetByEmail(string email)
    {
        string sql = $"SELECT * FROM {Table} WHERE email = @Email";
        return _connection.QueryFirstOrDefault<UserAccountModel>(sql, new { Email = email });
    }

    public void Update(UserAccountModel account)
    {
        string sql = $"UPDATE {Table} SET email = @Email, password = @Password, fullname = @FullName, dateofbirth = @DateOfBirth WHERE AccountId = @AccountId";
        _connection.Execute(sql, account);
    }

    public void Delete(UserAccountModel account)
    {
        string sql = $"DELETE FROM {Table} WHERE AccountId = @AccountId";
        _connection.Execute(sql, account);
    }

}