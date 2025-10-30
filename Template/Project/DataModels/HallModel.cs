public class HallModel
{

    public long HallId { get; set; }


    public HallModel(long hallid, long seatsamount)
    {
        HallId = hallid;
    }
}