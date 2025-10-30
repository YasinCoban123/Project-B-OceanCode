public class SeatTypeModel
{
    public long SeatId { get; set; }
    public string TypeName { get; set; }
    public double Price { get; set; }

    public SeatTypeModel()
    {

    }
    public SeatTypeModel(long seatid, string typename, double price)
    {
        SeatId = seatid;
        TypeName = typename;
        Price = price;
    }
}