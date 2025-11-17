using Microsoft.Data.Sqlite;

using Dapper;


public class UserAccountsAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private string Table = "UserAccount";

    public void Write(UserAccountModel account)
    {
        string sql = $"INSERT INTO {Table} (email, password, fullname, dateofbirth, isadmin) VALUES (@Email, @Password, @FullName, @DateOfBirth, @IsAdmin)";
        _connection.Execute(sql, account);
    }

    public UserAccountModel GetByEmail(string email)
    {
        string sql = $"SELECT * FROM {Table} WHERE email = @Email";
        return _connection.QueryFirstOrDefault<UserAccountModel>(sql, new { Email = email });
    }

    public List<UserAccountModel> GetAllUserAccounts()
    {
        string sql = $"SELECT * FROM {Table} WHERE IsAdmin = 0";
        return _connection.Query<UserAccountModel>(sql).ToList();
    }

    public UserAccountModel GetAccountByID(int id)
    {
        string sql = $"SELECT * FROM {Table} WHERE AccountId = @AccountId";
        return _connection.QueryFirstOrDefault<UserAccountModel>(sql, new { AccountId = id });     
    }


    public void Update(UserAccountModel account)
    {
        string sql = $"UPDATE {Table} SET email = @Email, password = @Password, fullname = @FullName, dateofbirth = @DateOfBirth WHERE AccountId = @AccountId";
        _connection.Execute(sql, account);
    }

    public bool Delete(UserAccountModel account)
    {
        string sql = $"DELETE FROM {Table} WHERE AccountId = @AccountId";

        try
        {
            _connection.Execute(sql, account);
            return true;
        }
        catch (Microsoft.Data.Sqlite.SqliteException ex)
        {
            if (ex.SqliteErrorCode == 19)
            {
                return false;
            }
        }
        return false;
    }


}