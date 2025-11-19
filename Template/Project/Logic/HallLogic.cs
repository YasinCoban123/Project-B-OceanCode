public class HallLogic
{
    public HallAcces _acces = new();

    public List<HallModel> GetAllHalls()
    {
        return _acces.GetAllHalls();
    }

    public bool CheckDate(string date)
    {
        if (date.Length != 10 || date[4] != '-' || date[7] != '-')
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
        if (time.Length != 8 || time[2] != ':' || time[5] != ':')
        {
            return false;
        }
        return true;
    }


}