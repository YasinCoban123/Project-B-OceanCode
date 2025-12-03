public class Hall
{
    static HallLogic logic = new();
    static SeatLogic seatLogic = new();
    static List<HallModel> AllHalls => logic.GetAllHalls();
    public static void Start()
    {
        Console.WriteLine();
        Console.WriteLine("[1] See all Halls");
        Console.WriteLine("[2] Add a Hall");
        Console.WriteLine("[3] Delete a Hall");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            SeeAllHalls();
        }

        if (choice == "2")
        {
            CreateAHall();
        }

        if (choice == "3")
        {
            DeleteAHall();
        }
        
    }

    public static void SeeAllHalls()
    {
        foreach(HallModel Hall in AllHalls)
        {
            Console.WriteLine($"Hall ID: {Hall.HallId}");
        }
        
    }
    public static void CreateAHall()
    {
        Console.WriteLine();
        HallModel Hall = logic.CreateAHall();
        Console.WriteLine();

        Console.WriteLine("Which Hall blueprint do you want to choose\nHall 1 \nHall 2 \nHall 3 ");
        string chosenHallBlueprintId = Console.ReadLine();
        long HallBlueprintId = Convert.ToInt64(chosenHallBlueprintId);

        HallModel ChosenHallBlueprint = AllHalls.Find(x => HallBlueprintId == x.HallId);
        seatLogic.DuplicateSeatsByHall(ChosenHallBlueprint, Hall.HallId);

        Console.WriteLine($"Successfully created Hall {Hall.HallId}");
        Console.WriteLine("Press ENTER to continue");
        Console.ReadLine();
        Console.Clear();

    }

    public static void DeleteAHall()
    {
        SeeAllHalls();
        Console.WriteLine();
        Console.WriteLine("Choose the Hall you want to delete");
        string choice = Console.ReadLine();
        long chosenHallId = Convert.ToInt64(choice);

        HallModel ChosenHallBlue = AllHalls.Find(x => chosenHallId == x.HallId);

        logic.DeleteHallAndSeats(ChosenHallBlue);
        Console.WriteLine();
        Console.WriteLine("Succedfully deleted all the seats and the hall!");
        Console.WriteLine("Press ENTER to continue");
        Console.ReadLine();
        Console.Clear();

    }

}