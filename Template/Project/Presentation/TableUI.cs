public class TableUI<T> where T : class
{
    public string Title { get; }
    public Dictionary<string, string> Headers { get; }
    public HashSet<string> FilterableHeaders { get; }
    public List<T> Data { get; }
    private List<T> FilteredData { get; set; }
    private Dictionary<string, Func<T, string>> ValueMappers { get; }

    public int SelectedColumn { get; private set; } = 0;
    public int SelectedIndex { get; private set; } = 0;
    public string SelectedFilterProperty { get; private set; }
    public string FilterInput { get; private set; }
    public T? Value{ get; private set; }

    private bool exitLoop = false;
    private bool ascending = true;
    private string sortProperty = null;
    private Dictionary<string, string> currentFilters = new();

    public TableUI(
        string title,
        Dictionary<string, string> headers,
        List<T> data,
        IEnumerable<string> filterHeaders,
        Dictionary<string, Func<T, string>>? valueMappers = null)
    {
        // Set the table title
        Title = title;

        // Set the headers dictionary (property name -> display name)
        Headers = headers;

        // Convert the input data to a list for easy indexing and manipulation
        Data = data;
        FilteredData = data;

        // Create a HashSet of filterable property names for quick lookup
        FilterableHeaders = new HashSet<string>(filterHeaders);

        ValueMappers = valueMappers ?? new Dictionary<string, Func<T, string>>();

        // Set the initial selected index:
        // -1 is the header row, 
        // -2, -3, ... are the filter options above the header,
        // so starting at -1 - FilterableHeaders.Count puts the cursor on the topmost filter
        SelectedIndex = -1 - FilterableHeaders.Count;
    }


    public T? Start()
    {
        while (!exitLoop)
        {
            Console.Clear();
            Console.WriteLine($"Sorting on: {sortProperty ?? "None"} ({(ascending ? "Ascending" : "Descending")})");
            Console.WriteLine();

            PrintFilterOptions();
            PrintTitle();
            PrintHeaderRow();
            PrintRows();
            PrintFooter();

            HandleInput();
        }

        return Value;
    }

    // PRINTING

    private Dictionary<string, int> GetColumnWidths()
    {
        var widths = new Dictionary<string, int>();

        foreach (var header in Headers)
        {
            int width = 16;

            foreach (var item in Data)
            {
                string value = GetCellValue(item, header.Key);

                if (DateTime.TryParse(value, out _))
                {
                    width = 32; // 30 chars + padding
                    break;
                }
            }

            widths[header.Key] = width;
        }

        return widths;
    }

    private string GetCellValue(T item, string property)
    {
        if (ValueMappers.TryGetValue(property, out var mapper))
            return mapper(item) ?? string.Empty;

        return typeof(T).GetProperty(property)?.GetValue(item)?.ToString() ?? string.Empty;
    }

    private void PrintTitle()
    {
        var widths = GetColumnWidths();
        int tableWidth = widths.Values.Sum() + 1;

        int padding = Math.Max(0, (tableWidth - Title.Length) / 2);

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(new string(' ', padding) + Title);
        Console.WriteLine(new string('═', tableWidth));
        Console.ResetColor();
    }

    private void PrintFilterOptions()
    {
        int idx = -1 - FilterableHeaders.Count;

        foreach (var filter in FilterableHeaders)
        {
            if (SelectedIndex == idx)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            currentFilters.TryGetValue(filter, out var value);
            Console.WriteLine($"Filter on {filter}: {value}");
            Console.ResetColor();
            idx++;
        }

        Console.WriteLine();
    }

    private void PrintHeaderRow()
    {
        var widths = GetColumnWidths();

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("╔" + string.Join("╦", Headers.Select(h => new string('═', widths[h.Key]))) + "╗");

        Console.Write("║");
        int colIndex = 0;

        foreach (var h in Headers)
        {
            int innerWidth = widths[h.Key] - 2;
            string text = h.Value ?? "";

            if (SelectedIndex == -1 && colIndex == SelectedColumn)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Console.Write(" " + text.PadRight(innerWidth) + " ");
            Console.ResetColor();
            Console.Write("║");
            colIndex++;
        }

        Console.WriteLine();
        Console.WriteLine("╠" + string.Join("╬", Headers.Select(h => new string('═', widths[h.Key]))) + "╣");
        Console.ResetColor();
    }

    private void PrintRows()
    {
        var widths = GetColumnWidths();
        FilteredData = ApplyFilters();

        for (int i = 0; i < FilteredData.Count; i++)
        {
            if (SelectedIndex == i)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Console.Write("║");

            foreach (var h in Headers)
            {
                int innerWidth = widths[h.Key] - 2;
                string value = GetCellValue(FilteredData[i], h.Key);

                int trimLength = DateTime.TryParse(value, out _) ? 30 : innerWidth;
                value = TrimToLength(value, trimLength);

                Console.Write(" " + value.PadRight(innerWidth) + " ");
                Console.Write("║");
            }

            Console.ResetColor();
            Console.WriteLine();
        }
    }

    private void PrintFooter()
    {
        var widths = GetColumnWidths();

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("╚" + string.Join("╩", Headers.Select(h => new string('═', widths[h.Key]))) + "╝");
        Console.ResetColor();

        Console.WriteLine();

        if (SelectedIndex == ApplyFilters().Count)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        Console.WriteLine("Go Back");
        Console.ResetColor();
    }

    private string TrimToLength(string str, int length)
    {
        if (str.Length <= length) return str;
        return str.Substring(0, length - 1) + "…";
    }

    // INPUT

    private void HandleInput()
    {
        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                SelectedIndex--;
                if (SelectedIndex < (-1 - FilterableHeaders.Count))
                    SelectedIndex = ApplyFilters().Count;
                break;

            case ConsoleKey.DownArrow:
                SelectedIndex++;
                if (SelectedIndex > ApplyFilters().Count)
                    SelectedIndex = -1 - FilterableHeaders.Count;
                break;

            case ConsoleKey.LeftArrow:
                if (SelectedIndex == -1)
                {
                    SelectedColumn--;
                    if (SelectedColumn < 0) SelectedColumn = Headers.Count - 1;
                }
                break;

            case ConsoleKey.RightArrow:
                if (SelectedIndex == -1)
                {
                    SelectedColumn++;
                    if (SelectedColumn >= Headers.Count) SelectedColumn = 0;
                }
                break;

            case ConsoleKey.Enter:
                HandleEnter();
                break;
        }
    }

    private void HandleEnter()
    {
        // Header selected → sort
        if (SelectedIndex == -1)
        {
            string col = Headers.ElementAt(SelectedColumn).Key;
            SortBy(col);
            return;
        }

        // Filter option selected → input
        int filterStart = -1 - FilterableHeaders.Count;
        if (SelectedIndex < -1 && SelectedIndex >= filterStart)
        {
            int filterIdx = SelectedIndex - filterStart;
            string filterProp = FilterableHeaders.ElementAt(filterIdx);

            Console.SetCursorPosition(0, FilterableHeaders.Count + 5);
            Console.Clear();
            Console.Write($"Enter filter value for {filterProp}: ");
            string input = Console.ReadLine();

            currentFilters[filterProp] = input;
            SelectedFilterProperty = filterProp;
            FilterInput = input;
            return;
        }

        // Go Back selected
        if (SelectedIndex == ApplyFilters().Count)
        {
            Value = null;
            exitLoop = true;
            return;
        }

        // Selecting a row → exit
        Value = FilteredData[SelectedIndex];
        exitLoop = true;
    }

    private void SortBy(string property)
    {
        sortProperty = property;

        var prop = typeof(T).GetProperty(property);
        if (prop == null)
            return; // Property doesn't exist


        Data.Sort((a, b) =>
        {
            object valueA = prop.GetValue(a);
            object valueB = prop.GetValue(b);

            // Compare the two values
            int result = Comparer<object>.Default.Compare(valueA, valueB);

            // Flip the result if sorting descending
            if (!ascending)
                result = -result;

            return result;
        });

        // Next time, flip sorting direction
        ascending = !ascending;
    }


    private List<T> ApplyFilters()
    {
        List<T> result = new List<T>();

        foreach (var item in Data)
        {
            bool matchesAllFilters = true;

            foreach (var filter in currentFilters)
            {
                string propertyName = filter.Key;
                string filterValue = filter.Value;

                // Skip filters with no value
                if (string.IsNullOrWhiteSpace(filterValue))
                    continue;

                // Get the property info
                var property = typeof(T).GetProperty(propertyName);
                if (property == null)
                    continue;

                // Get the item's value as a string
                string itemValue = property.GetValue(item)?.ToString() ?? string.Empty;

                if (!itemValue.Contains(filterValue, StringComparison.OrdinalIgnoreCase))
                {
                    matchesAllFilters = false;
                    break;
                }
            }

            if (matchesAllFilters)
                result.Add(item);
        }

        return result;
    }
}
