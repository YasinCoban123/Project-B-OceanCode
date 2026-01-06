public class SeatSelectionHelper
{
    private readonly List<SeatRowLogic> _rows;
    private readonly List<int> _selected = new();

    private int rowIndex = 0;
    private int seatIndex = 0;

    public SeatSelectionHelper(List<SeatRowLogic> rows)
    {
        _rows = rows;
    }

    public List<int> StartSelection()
    {
        ConsoleKey key;
        Console.CursorVisible = false;

        do
        {
            Draw();
            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    seatIndex = Math.Max(0, seatIndex - 1);
                    break;

                case ConsoleKey.RightArrow:
                    seatIndex = Math.Min(_rows[rowIndex].Seats.Count - 1, seatIndex + 1);
                    break;

                case ConsoleKey.UpArrow:
                    rowIndex = Math.Max(0, rowIndex - 1);
                    seatIndex = Math.Min(seatIndex, _rows[rowIndex].Seats.Count - 1);
                    break;

                case ConsoleKey.DownArrow:
                    rowIndex = Math.Min(_rows.Count - 1, rowIndex + 1);
                    seatIndex = Math.Min(seatIndex, _rows[rowIndex].Seats.Count - 1);
                    break;

                case ConsoleKey.Enter:
                    ToggleSeat();
                    break;
            }

        } 
        
        while (key != ConsoleKey.Escape);

        Console.CursorVisible = true;
        Console.Clear();

        return new List<int>(_selected);
    }

    private void ToggleSeat()
    {
        var seat = _rows[rowIndex].Seats[seatIndex];
        
        if (seat.IsTaken) 
        {
            return;
        }
        
        int seatId = (int)seat.SeatId;

        if (_selected.Contains(seatId))
        {
            _selected.Remove(seatId);
        }
        else
        {
            if (_selected.Count >= 20) 
            {
                return;
            }
            _selected.Add(seatId);
        }
    }

    private void Draw()
    {
        Console.Clear();
        Console.WriteLine("Select seats (ENTER = select, ESC = confirm)");
        Console.WriteLine($"Selected: {_selected.Count}/20\n");

        for (int r = 0; r < _rows.Count; r++)
        {
            string rowLabel = $"Row {_rows[r].RowNumber}: ";
            int consoleWidth = Console.WindowWidth;

            string preview = "";
            foreach (var _ in _rows[r].Seats)
            {
                preview += "[XXXX]";
            }

            int seatWidth = preview.Length;
            int leftPadding = (consoleWidth - rowLabel.Length - seatWidth) / 2;

            if (leftPadding < 0) 
            {
                leftPadding = 0;
            }

            Console.Write(rowLabel);
            Console.Write(new string(' ', leftPadding));

            for (int s = 0; s < _rows[r].Seats.Count; s++)
            {
                var seat = _rows[r].Seats[s];
                bool cursor = r == rowIndex && s == seatIndex;
                bool selected = _selected.Contains((int)seat.SeatId);

                if (cursor)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                }
                if (seat.IsTaken)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[XXX]");
                }
                else
                {
                    Console.ForegroundColor = GetSeatColor(seat.TypeName, selected);
                    Console.Write($"[{seat.SeatId.ToString().PadLeft(3)}]");
                }

                Console.ResetColor();
            }

            Console.WriteLine();
        }

        Console.WriteLine("\nLegend:");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Normal ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Relax ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("VIP ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Selected");
        Console.ResetColor();
    }

    private ConsoleColor GetSeatColor(string type, bool selected)
    {
        if (selected) 
        {
            return ConsoleColor.Cyan;
        }

        return type switch
        {
            "Relax" => ConsoleColor.Yellow,
            "VIP" => ConsoleColor.Magenta,
            _ => ConsoleColor.Green
        };
    }
}
