using Microsoft.Data.Sqlite;
using Dapper;

public class HallAcces : DataAccess
{
    protected override string Table => "Hall";

    public List<HallModel> GetAllHalls()
    {
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<HallModel>(sql).ToList();;
    }
    public HallModel Write()
    {
        string sql = $"INSERT INTO {Table} DEFAULT VALUES returning *;";
        return _connection.QueryFirstOrDefault<HallModel>(sql);
    }


    // public HallModel GetHighestIdHall()
    // {
    //     string sql = $"SELECT * FROM {Table} ORDER BY HallId DESC LIMIT 1";
    //     return _connection.ExecuteScalar<HallModel>(sql);
    // }

    public void Delete(HallModel Hall)
    {
        string sql = $"DELETE FROM {Table} WHERE HallId = @HallId";
        _connection.Execute(sql, Hall);
    }


}