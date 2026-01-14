static class Feedback
{
    static private FeedbackLogic feedbackLogic = new();

    public static void Start()
    {
        Console.Clear();

        MenuHelper menu = new MenuHelper(new[]
        {
            "Send a Tip",
            "Send a Top",
            "Return back to Menu"
        },
        "Screenings Admin");

        menu.Show();

        int choice = menu.SelectedIndex;

        Console.Clear();

        if (choice == 0)
        {
            Console.Clear();
            int num = 1;
            SendFeedback(num);
        }
        else if (choice == 1)
        {
            Console.Clear();
            int num = 0;
            SendFeedback(num);
        }
        else if (choice == 2)
        {
            Console.Clear();
            Menu.AdminStart();
        }
    }

    private static void SendFeedback(int num)
    {
        Console.WriteLine("Write down your Feedback under here please. Press ENTER to stop.");
        Console.WriteLine("Message needs to be between 5/150 characters long");
        Console.ForegroundColor = ConsoleColor.Cyan;
        string fdback = Console.ReadLine()!;
        bool result = feedbackLogic.SendFeedbackLogic(fdback, num);
        Console.ResetColor();

        if (!result)
        {
            Console.WriteLine("Message sent is not between the character limit. please resend it.");
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            Console.Clear();
            SendFeedback(num);
        }
        else
        {
            Console.WriteLine("Message send succesfully, Thanks for the feedback");
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();            
            Console.Clear();
            Start();
        }
    }

}
