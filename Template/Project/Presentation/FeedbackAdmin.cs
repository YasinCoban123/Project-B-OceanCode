static class FeedbackAdmin
{
    static private FeedbackLogic feedbackLogic = new();

    public static void Start()
    {
        Console.Clear();

        MenuHelper menu = new MenuHelper(new[]
        {
            "View Tips",
            "View Tops",
            "Return back to Menu"
        },
        "Screenings Admin");

        menu.Show();

        int choice = menu.SelectedIndex;

        Console.Clear();

        if (choice == 1)
        {
            Console.Clear();
            int num = 0;
            ViewFeedback(num);
        }
        else if (choice == 0)
        {
            Console.Clear();
            int num = 1;
            ViewFeedback(num);
        }
        else if (choice == 2)
        {
            Console.Clear();
            Menu.Start();
        }
    }

    public static void ViewFeedback(int num)
    {
        List<FeedbackModel> feedlist = feedbackLogic.ViewFeedbackLogic(num);
        if (feedlist.Count() == 0)
        {
            Console.WriteLine("No Feedback in Database");
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
            Console.Clear();
            Start();
        }

        int i = 0;
        foreach (var item in feedlist)
        {
            if (num == 1)
            {
                i++;
                Console.Write($"{i}: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{item.Feedback}\n");
                Console.WriteLine();
                Console.ResetColor();
            }
            else
            {
                i++;
                Console.Write($"{i}: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{item.Feedback}\n");
                Console.WriteLine();
                Console.ResetColor();
            }
        }
        Console.WriteLine("Press ENTER to continue");
        Console.ReadLine();
        Console.Clear();
        Start();
    }

    
}
