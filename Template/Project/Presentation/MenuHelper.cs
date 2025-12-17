using System;
using System.Collections.Generic;

public class MenuHelper
{
    public int SelectedIndex { get; private set; }
    public string SelectedValue => _options[SelectedIndex];

    private readonly List<string> _options;
    private readonly string _title;

    // Hoeveel regels menu gebruikt heeft — zodat we straks alles kunnen wissen
    private int lastRenderHeight = 0;

    public MenuHelper(IEnumerable<string> options, string title = "")
    {
        _options = new List<string>(options);
        _title = title;
    }

    public void Show()
    {
        Console.CursorVisible = false;
        int index = 0;
        ConsoleKey key;

        do
        {
            Draw(index);

            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow)
            {
                index = (index - 1 + _options.Count) % _options.Count;
                AudioLogic.PlayMoveSound();
            }

            if (key == ConsoleKey.DownArrow)
            {
                index = (index + 1) % _options.Count;
                AudioLogic.PlayMoveSound();
            }

        } while (key != ConsoleKey.Enter);

        SelectedIndex = index;
        Console.CursorVisible = true;

        ClearMenuPrint();   // <<< BELANGRIJK
    }

    private void Draw(int current)
    {
        Console.SetCursorPosition(0, 0);

        int menuHeight = _options.Count + 5;
        lastRenderHeight = menuHeight;

        // Wis menu-gebied
        for (int i = 0; i < menuHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(new string(' ', Console.WindowWidth));
        }

        Console.SetCursorPosition(0, 0);

        Header.PrintHeader();

        if (!string.IsNullOrWhiteSpace(_title))
        {
            Console.WriteLine(_title);
            Console.WriteLine();
        }

        Console.WriteLine("Use ↑ / ↓ and Enter");

        for (int i = 0; i < _options.Count; i++)
        {
            if (i == current)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.ResetColor();
            }

            Console.WriteLine(_options[i].PadRight(Console.WindowWidth));
        }

        Console.ResetColor();
    }

    private void ClearMenuPrint()
    {
        // Wis het hele menu-gebied schoon
        for (int i = 0; i < lastRenderHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(new string(' ', Console.WindowWidth));
        }

        // Cursor zetten net onder het menu zodat programma-uitvoer netjes start
        Console.SetCursorPosition(0, lastRenderHeight);
    }
}
