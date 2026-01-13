using System;
using NAudio.Wave;

public static class BGPlayer
{
    private static readonly string Jazz =
        @"https://drive.google.com/uc?export=download&id=1-oh7k7xUX-pWk_JKmQ5LGmRzgyKr0SHe";

    private static readonly string Action =
        @"https://drive.google.com/uc?export=download&id=1D5yzdAHv1FAfnSp_7AP_aFqTFru-Hi4j";

    private static readonly string Horror =
        @"https://drive.google.com/uc?export=download&id=12sKDfBezvHj3Mcu8XiBhalJe80oz4GfF";
    
    private static readonly string Documentary =
    "https://drive.google.com/uc?export=download&id=1QB9Twje5fkyVOflkkg1vsRzN8kuTPUUI";

    private static readonly string Medieval =
    "https://drive.google.com/uc?export=download&id=1U_wKyjbw2KWjES9JbrObh9wGFFyMS53L";

    private static readonly string YeSTFU =
    "https://drive.google.com/uc?export=download&id=1SigMiGPo-LJOicfDTVLvKvJ30bzrPdmb";



    private static WaveOutEvent outputDevice;
    private static AudioFileReader audioFile;
    

    public static void Main()
    {
        RunMenu();
    }

    private static void RunMenu()
    {
        MenuHelper menu;
        while (true)
        {
            menu = new MenuHelper(new[] {
                "Jazz",
                "Action",
                "Horror",
                "Documentary",
                "Medieval",
                "YESTFU",
                "Stop Music",
                "Exit",
            },
            "Welcome to the Music Menu");

            menu.Show();

            switch (menu.SelectedIndex)
            {
                case 0:
                    Play(Jazz);
                    break;

                case 1:
                    Play(Action);
                    break;

                case 2:
                    Play(Horror);
                    break;
                case 3:
                    Play(Documentary);
                    break;
                case 4:
                    Play(Medieval);
                    break;
                case 5:
                    Play(YeSTFU);
                    break;
                case 6:
                    Stop();
                    break;
                case 7:
                    return; // exits program
            }

            //Console.WriteLine("\nPress any key to return to menu...");
            //Console.ReadKey();
        }
    }

    private static void Play(string path)
    {
        Console.Clear();
        Stop();

        audioFile = new AudioFileReader(path);
        outputDevice = new WaveOutEvent();

        outputDevice.Init(audioFile);
        outputDevice.Play();

        //Console.WriteLine("Now playing...");
    }

    private static void Stop()
    {
        Console.Clear();
        outputDevice?.Stop();
        outputDevice?.Dispose();
        audioFile?.Dispose();

        outputDevice = null;
        audioFile = null;

        //Console.WriteLine("Music stopped.");
    }
}
