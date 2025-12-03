public class HallLogic
{
    public HallAcces _acces = new();
    public SeatAcces _seatacces = new();

    public List<HallModel> GetAllHalls()
    {
        return _acces.GetAllHalls();
    }

    // public HallModel GetHighestIdHall()
    // {
    //     return _acces.GetHighestIdHall();
    // }

    public HallModel CreateAHall()
    {
        HallModel hall = _acces.Write(); 
        return hall;
        
    }

    public bool CheckDate(string date)
    {
        if (date.Length != 10 || date[2] != '-' || date[5] != '-')
        {
            return false;
        }

        if (!DateTime.TryParse(date, out DateTime parsedDate))
        {
            return false;
        }

        if (parsedDate.Date < DateTime.Today)
        {
            return false;
        }

        return true;
    }

    public bool CheckTime(string time)
    {
        if (time.Length != 5 || time[2] != ':')
        {
            return false;
        }
        return true;
    }

    public void DeleteHallAndSeats(HallModel Hall)
    {
        _seatacces.Delete(Hall);
        _acces.Delete(Hall);
    }


}