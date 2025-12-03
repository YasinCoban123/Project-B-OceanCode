public class SeatLogic
{
    private static SeatAcces _acces = new();

    public List<SeatModel> GetAllSeatsByHallId(long hallid)
    {
        return _acces.GetAllSeatsByHallId(hallid);
    }

    public void DuplicateSeatsByHall(HallModel hall, long Hallid)
    {
        _acces.DuplicateSeatsByHall(hall, Hallid);
    }
}