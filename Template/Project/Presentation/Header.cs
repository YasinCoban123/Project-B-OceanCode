public static class Header
{
    public static void PrintHeader()
    {
        //ClearMenuPrint();
        Console.Clear();

        Console.WriteLine(
        @"
▄    ▄▄▄  ▄█████ ▄▄▄▄▄  ▄▄▄  ▄▄  ▄▄ ▄▄ ▄▄   ▄▄  ▄▄▄
 ▀▄ ██▀██ ██     ██▄▄  ██▀██ ███▄██ ██ ██▀▄▀██ ██▀██
▄▀  ▀███▀ ▀█████ ██▄▄▄ ██▀██ ██ ▀██ ██ ██   ██ ██▀██
        "
        );


        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
    }

    private static void ClearMenuPrint()
    {
        // Wis het hele menu-gebied schoon
        int lastRenderHeight = 99;
        for (int i = 0; i < lastRenderHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(new string(' ', Console.WindowWidth));
        }

        // Cursor zetten net onder het menu zodat programma-uitvoer netjes start
        Console.SetCursorPosition(0, lastRenderHeight);
    }
}