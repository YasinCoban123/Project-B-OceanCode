public class ReservationAdmin
{
    ScreeningLogic screeningLogic = new ScreeningLogic();

    public void Start()
    {
        MenuHelper menu = new MenuHelper(new[]{
            "See all reservations",
            "Edit user reservation",
            "Delete user reservation",
            "See statistics",
            "Go Back"
        },
        "Reservation Options");

        menu.Show();
        switch (menu.SelectedIndex)
        {
            case 0:
                List<ReservationModel> allReservations = screeningLogic.GetAllReservations();
                foreach (ReservationModel reservation in allReservations)
                {
                    Console.WriteLine();
                    Console.WriteLine($"ReservationId: {reservation.ReservationId}");
                    Console.WriteLine($"AccountId: {reservation.AccountId}");
                    Console.WriteLine($"ScreeningId: {reservation.ScreeningId}");
                    Console.WriteLine($"ReservationTime: {reservation.ReservationTime}");
                    Console.WriteLine("Press ENTER to continue");
                    Console.ReadLine();
                    Console.Clear();
                    Menu.AdminStart();
                }
                break;
            case 1:
                Console.WriteLine("Morgan Fraudhand is the biggest fraud in NC");
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
                Console.Clear();
                break;
            case 2:
                Console.WriteLine("Morgan Fraudhand is Finished");
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
                Console.Clear();
                break;
            case 3:
                Console.WriteLine("Jackie and V are the absolutes goat of NC");
                Console.WriteLine("Press ENTER to continue");
                Console.ReadLine();
                Console.Clear();
                break;
            case 4:
            Console.WriteLine("Panam the goat");
            break;

        }

        

    }
}