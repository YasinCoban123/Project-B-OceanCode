public class OptionsMenu
{
    private List<string> _options;
    private readonly string Title;
    public int Selected;
    public string SelectedText;
    public OptionsMenu(List<string> options, string title = "")
    {
        _options = options;
        Title = title;
        Run();
    }

    public void Run()
    {
        int selectedOption = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();
            if (Title != null || Title != "")
            {
                Console.WriteLine(Title);
                Console.WriteLine();
            }
            
            Console.WriteLine("Use ↑/↓ to navigate and Enter to select option");

            for (int i = 0; i < _options.Count; i++)
            {
                // Colors
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(_options[i]);
            }
            Console.ResetColor();

            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow)
            {
                selectedOption--;
                if (selectedOption < 0)
                    selectedOption = _options.Count - 1;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                selectedOption++;
                if (selectedOption >= _options.Count)
                    selectedOption = 0;
            }
        } while (key != ConsoleKey.Enter);

        Selected = selectedOption;
        SelectedText = _options[Selected];
    }
}