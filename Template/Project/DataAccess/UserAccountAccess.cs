using Microsoft.Data.Sqlite;

using Dapper;


public class AccountsAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private string Table = "Accounts";

    public void Write(UserAccountModel account)
    {
        string sql = $"INSERT INTO {Table} (email, password, fullname) VALUES (@EmailAddress, @Password, @FullName)";
        _connection.Execute(sql, account);
    }

    public UserAccountModel GetByEmail(string email)
    {
        string sql = $"SELECT * FROM {Table} WHERE email = @Email";
        return _connection.QueryFirstOrDefault<UserAccountModel>(sql, new { Email = email });
    }

    public void Update(UserAccountModel account)
    {
        string sql = $"UPDATE {Table} SET email = @EmailAddress, password = @Password, fullname = @FullName WHERE id = @Id";
        _connection.Execute(sql, account);
    }

    public void Delete(UserAccountModel account)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @Id";
        _connection.Execute(sql, new { Id = account.Id });
    }



}