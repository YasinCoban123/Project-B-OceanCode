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
        while (true)
        {
            Console.WriteLine();
            HallModel hall = logic.CreateAHall();
            Console.WriteLine();

            MenuHelper menu = new MenuHelper(
                new[]
                {
                    "Hall 1",
                    "Hall 2",
                    "Hall 3"
                },
                "Which Hall blueprint do you want to choose:"
            );

            menu.Show();

            int hallBlueprintId = menu.SelectedIndex + 1;

            List<SeatRowLogic> seatrows = Screenings.GetSeatRowsOrReturn(hallBlueprintId);
            Screenings.PrintSeatRows(seatrows);
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
            Console.Clear();


            MenuHelper choiceMenu = new MenuHelper(
                new[]
                {
                    "Yes",
                    "No",
                },
                "Do you want this seat layout?"
            );

            choiceMenu.Show();

            if (choiceMenu.SelectedIndex == 0)
            {
                HallModel chosenHallBlueprint =
                    AllHalls.Find(x => x.HallId == hallBlueprintId);

                seatLogic.DuplicateSeatsByHall(chosenHallBlueprint, hall.HallId);

                Console.Clear();
                Console.WriteLine($"Successfully created Hall {hall.HallId}");
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
                Console.Clear();

                break;
            }

            Console.Clear();
        }
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

        if (ChosenHallBlue.HallId == 1 || ChosenHallBlue.HallId == 2 || ChosenHallBlue.HallId == 3)
        {
            Console.WriteLine("Blueprint Halls 1,2,3 cannot be deleted!");
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
            Console.Clear();
        }
        else
        {
            logic.DeleteHallAndSeats(ChosenHallBlue);
            Console.WriteLine();
            Console.WriteLine("Succedfully deleted all the seats and the hall!");
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
            Console.Clear();
        }

        
    }

}