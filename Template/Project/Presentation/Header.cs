public static class Header
{
    public static void PrintHeader()
    {
        //ClearMenuPrint();
        Console.Clear();

        Console.WriteLine("                                                                                                                                                                                       ");
        Console.WriteLine("                                                                                                                                                                                       ");
        Console.WriteLine("                                        OOOOOOOOO                  CCCCCCCCCCCCC                                                       iiii                                            ");
        Console.WriteLine("     >>>>>>>                          OO:::::::::OO             CCC::::::::::::C                                                      i::::i                                           ");
        Console.WriteLine("      >:::::>                       OO:::::::::::::OO         CC:::::::::::::::C                                                       iiii                                            ");
        Console.WriteLine("       >:::::>                     O:::::::OOO:::::::O       C:::::CCCCCCCC::::C                                                                                                       ");
        Console.WriteLine("        >:::::>    =============== O::::::O   O::::::O      C:::::C       CCCCCC    eeeeeeeeeeee    aaaaaaaaaaaaa  nnnn  nnnnnnnn    iiiiiii    mmmmmmm    mmmmmmm     aaaaaaaaaaaaa   ");
        Console.WriteLine("         >:::::>   =:::::::::::::= O:::::O     O:::::O     C:::::C                ee::::::::::::ee  a::::::::::::a n:::nn::::::::nn  i:::::i  mm:::::::m  m:::::::mm   a::::::::::::a  ");
        Console.WriteLine("          >:::::>  =============== O:::::O     O:::::O     C:::::C               e::::::eeeee:::::eeaaaaaaaaa:::::an::::::::::::::nn  i::::i m::::::::::mm::::::::::m  aaaaaaaaa:::::a ");
        Console.WriteLine("           >:::::>                 O:::::O     O:::::O     C:::::C              e::::::e     e:::::e         a::::ann:::::::::::::::n i::::i m::::::::::::::::::::::m           a::::a ");
        Console.WriteLine("          >:::::>  =============== O:::::O     O:::::O     C:::::C              e:::::::eeeee::::::e  aaaaaaa:::::a  n:::::nnnn:::::n i::::i m:::::mmm::::::mmm:::::m    aaaaaaa:::::a ");
        Console.WriteLine("         >:::::>   =:::::::::::::= O:::::O     O:::::O     C:::::C              e:::::::::::::::::e aa::::::::::::a  n::::n    n::::n i::::i m::::m   m::::m   m::::m  aa::::::::::::a ");
        Console.WriteLine("        >:::::>    =============== O:::::O     O:::::O     C:::::C              e::::::eeeeeeeeeee a::::aaaa::::::a  n::::n    n::::n i::::i m::::m   m::::m   m::::m a::::aaaa::::::a ");
        Console.WriteLine("       >:::::>                     O::::::O   O::::::O      C:::::C       CCCCCCe:::::::e         a::::a    a:::::a  n::::n    n::::n i::::i m::::m   m::::m   m::::ma::::a    a:::::a ");
        Console.WriteLine("      >:::::>                      O:::::::OOO:::::::O       C:::::CCCCCCCC::::Ce::::::::e        a::::a    a:::::a  n::::n    n::::ni::::::im::::m   m::::m   m::::ma::::a    a:::::a ");
        Console.WriteLine("     >>>>>>>                        OO:::::::::::::OO         CC:::::::::::::::C e::::::::eeeeeeeea:::::aaaa::::::a  n::::n    n::::ni::::::im::::m   m::::m   m::::ma:::::aaaa::::::a ");
        Console.WriteLine("                                      OO:::::::::OO             CCC::::::::::::C  ee:::::::::::::e a::::::::::aa:::a n::::n    n::::ni::::::im::::m   m::::m   m::::m a::::::::::aa:::a");
        Console.WriteLine("                                        OOOOOOOOO                  CCCCCCCCCCCCC    eeeeeeeeeeeeee  aaaaaaaaaa  aaaa nnnnnn    nnnnnniiiiiiiimmmmmm   mmmmmm   mmmmmm  aaaaaaaaaa  aaaa");

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