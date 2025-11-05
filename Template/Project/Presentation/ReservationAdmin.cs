public class ReservationAdmin
{
    ScreeningLogic screeningLogic = new ScreeningLogic();

    public void Start()
    {
        Console.WriteLine();
        Console.WriteLine("[1] See all Reservations");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            List<ReservationModel> allReservations = screeningLogic.GetAllReservations();
            foreach (ReservationModel reservation in allReservations)
            {
                Console.WriteLine();
                Console.WriteLine($"ReservationId: {reservation.ReservationId}");
                Console.WriteLine($"AccountId: {reservation.AccountId}");
                Console.WriteLine($"ScreeningId: {reservation.ScreeningId}");
                Console.WriteLine($"ReservationTime: {reservation.ReservationTime}");
            }

        }

    }
}