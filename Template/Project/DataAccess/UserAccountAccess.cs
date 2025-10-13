using Microsoft.Data.Sqlite;

using Dapper;


public class UserAccountsAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private string Table = "UserAccount";

    public void Write(UserAccountModel account)
    {
        string sql = $"INSERT INTO {Table} (email, password, fullname, dateofbirth) VALUES (@EmailAddress, @Password, @FullName, @DateOfBirth)";
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

    public Dictionary<string, string> EmailPasswordDict()
    {
        Dictionary<string, string> accounts = new();

        string sql = $"SELECT email, password FROM {Table}";
        var result = _connection.Query(sql);

        foreach (var row in result)
        {
            accounts[row.Email] = row.Password;
        }
        
        return accounts;
    }


    public List<string> GetAllEmails()
    {
        List<string> emails = new();

        string sql = $"SELECT email FROM {Table}";
        var result = _connection.Query(sql);

        foreach (var row in result)
        {
            string email = row?.email;
            if (email != null)
                emails.Add(email);
        }
        return emails;
    }

}