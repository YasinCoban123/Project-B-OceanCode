using Microsoft.Data.Sqlite;
using Dapper;
public class FeedbackAcces
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private const string Table = "Feedback";

    public void SendFeedbackAcces(string feedback, int num)
    {
        string sql = $"INSERT INTO {Table} (Feedback, IsTip) VALUES (@Feedback, {num})";
        _connection.Execute(sql, new { Feedback = feedback });
    }

    public List<FeedbackModel> ViewFeedback(int num)
    {
        string sql = $@"
            SELECT FeedbackId, Feedback, IsTip
            FROM {Table}
            WHERE IsTip = {num}
            ORDER BY FeedbackId ASC;
        ";

        return _connection.Query<FeedbackModel>(sql).ToList();
    }
    
}
