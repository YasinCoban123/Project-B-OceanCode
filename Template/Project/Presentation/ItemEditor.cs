public class EditOption<T>
{
    public string Label { get; set; }
    public Func<T, string> Display { get; set; }

    public Func<T, string, (bool ok, string error)> TryApply { get; set; }
}


public class ItemEditor<T>
{
    public T Item;
    public readonly string _title;
    private readonly List<EditOption<T>> _options;

    public ItemEditor(T item, string title, List<EditOption<T>> options)
    {
        Item = item;
        _title = title;
        _options = options;
    }

    public T Start()
    {
        int index = 0;
        bool confirmed = false;
        var lineTops = new List<int>();

        while (!confirmed)
        {
            Console.Clear();
            Header.PrintHeader();
            Console.WriteLine(_title);

            lineTops.Clear();

            for (int i = 0; i < _options.Count; i++)
            {
                lineTops.Add(Console.CursorTop);

                var opt = _options[i];
                string text = $"{opt.Label}: {opt.Display?.Invoke(Item) ?? ""}";

                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }

                Console.WriteLine(text);
                Console.ResetColor();
            }

            Console.WriteLine("\n[Enter] Edit | [C] Confirm");

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.UpArrow)
                index = (index - 1 + _options.Count) % _options.Count;

            else if (key.Key == ConsoleKey.DownArrow)
                index = (index + 1) % _options.Count;

            else if (key.Key == ConsoleKey.Enter)
            {
                var opt = _options[index];
                int top = lineTops[index];
                int valueLeft = opt.Label.Length + 2;
            
                Console.CursorVisible = true;
            
                while (true)
                {
                    // clear old value
                    Console.SetCursorPosition(valueLeft, top);
                    Console.Write(new string(' ', 30));
                    Console.SetCursorPosition(valueLeft, top);
            
                    string input = Console.ReadLine();
            
                    var result = opt.TryApply(Item, input);
            
                    if (result.ok)
                        break;
            
                    // show error just below
                    Console.SetCursorPosition(0, top + 1);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(result.error.PadRight(Console.WindowWidth));
                    Console.ResetColor();
                }
            
                Console.CursorVisible = false;
            }

            else if (key.Key == ConsoleKey.C)
                confirmed = true;
        }

        return Item; // return updated item
    }
}
