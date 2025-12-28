public class FeedbackModel
{

    public long FeedbackId { get; set; }
    public string Feedback { get; set; }
    public bool IsTip { get; set; }


    public FeedbackModel()
    {

    }
    public FeedbackModel(long feedbackId, string feedback, bool istip)
    {
        FeedbackId = feedbackId;
        Feedback = feedback;
        IsTip = istip;
    }
}