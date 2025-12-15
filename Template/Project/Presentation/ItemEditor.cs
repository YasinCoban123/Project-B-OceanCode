public class EditOption<T>
{
    public string Label { get; set; }
    public Func<T, string> Display { get; set; }
    public Action<T> OnSelect { get; set; }
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

        while (!confirmed)
        {
            Console.Clear();
            Console.WriteLine(_title);

            for (int i = 0; i < _options.Count; i++)
            {
                var opt = _options[i];
                string text = $"{opt.Label}: {opt.Display?.Invoke(Item) ?? ""}";

                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }

                Console.WriteLine($"{text}");

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
                Console.Clear();
                _options[index].OnSelect?.Invoke(Item);
            }

            else if (key.Key == ConsoleKey.C)
                confirmed = true;
        }

        return Item; // return updated item
    }
}
