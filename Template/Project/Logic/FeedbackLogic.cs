public class FeedbackLogic
{
    private FeedbackAcces _feedbackAcces = new FeedbackAcces();

    public bool SendFeedbackLogic(string feedback, int num)
    {
        if (feedback.Length < 5 || feedback.Length > 150)
        {
            return false;
        }
        else
        {
            _feedbackAcces.SendFeedbackAcces(feedback, num);
            return true;
        }
    }

    public List<FeedbackModel> ViewFeedbackLogic(int num)
    {
        List<FeedbackModel> result = _feedbackAcces.ViewFeedback(num);
        return result;
    }
}
