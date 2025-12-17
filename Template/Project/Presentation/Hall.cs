public class Hall
{
    static HallLogic logic = new();
    static SeatLogic seatLogic = new();
    static List<HallModel> AllHalls => logic.GetAllHalls();
    public static void Start()
    {
        Console.WriteLine();
        
        MenuHelper menu = new MenuHelper(new[]
        {
            "See all Halls",
            "Add a Hall",
            "Delete a Hall",
            "Go Back"
        },
        "Hall Menu");

        menu.Show();
        int choice = menu.SelectedIndex;

        Console.Clear();

        if (choice == 0)
        {
            SeeAllHalls();
        }

        if (choice == 1)
        {
            CreateAHall();
        }

        if (choice == 2)
        {
            DeleteAHall();
        }

        if (choice == 3)
        {
            Console.Clear();
            Menu.AdminStart();
        }
    }

    public static void SeeAllHalls()
    {
        var table = new TableUI<HallModel>(
            "All halls", 
            new(
                [new("HallId", "ID")
            ]),
            AllHalls,
            ["HallId"]);
        table.Start();
        Console.Clear();
    }

    public static void CreateAHall()
    {
        Console.WriteLine();
        HallModel Hall = logic.CreateAHall();
        Console.WriteLine();

        Console.WriteLine("Which Hall blueprint do you want to choose\nHall 1 \nHall 2 \nHall 3 ");
        MenuHelper menu = new MenuHelper(new[]
            {
                "Hall 1",
                "Hall 2",
                "Hall 3"
            },
            "Which Hall blueprint do you want to choose:");
        string chosenHallBlueprintId = menu.SelectedIndex + 1.ToString();
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
        var table = new TableUI<HallModel>(
            "Choose the Hall you want to delete", 
            new(
                [new("HallId", "ID")
            ]),
            AllHalls,
            ["HallId"]);
        
        HallModel? hall = table.Start();

        if (hall is null)
        {
            return;
        }

        long chosenHallId = hall.HallId;
        HallModel? ChosenHallBlue = AllHalls.Find(x => chosenHallId == x.HallId);

        logic.DeleteHallAndSeats(ChosenHallBlue);
        Console.WriteLine();
        Console.WriteLine("Succedfully deleted all the seats and the hall!");
        Console.WriteLine("Press ENTER to continue");
        Console.ReadLine();
        Console.Clear();
    }

}