using System;
using NAudio.Wave;

public static class BGPlayer
{
    private static readonly string Jazz =
        @"C:\Users\amine\OneDrive\Documenten\GitHub\Project-B-OceanCode\Template\Project\AudioPlayer\MusicBG\cool-jazz-loops-2641.mp3";

    private static readonly string Action =
        @"C:\Users\amine\OneDrive\Documenten\GitHub\Project-B-OceanCode\Template\Project\AudioPlayer\MusicBG\racing-speed-action-music-416097.mp3";

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
                    Stop();
                    break;

                case 3:
                    return; // exits program
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
    }

    private static void Play(string path)
    {
        Console.Clear();
        Stop(); // stop previous music first

        audioFile = new AudioFileReader(path);
        outputDevice = new WaveOutEvent();

        outputDevice.Init(audioFile);
        outputDevice.Play();

        Console.WriteLine("Now playing...");
    }

    private static void Stop()
    {
        Console.Clear();
        outputDevice?.Stop();
        outputDevice?.Dispose();
        audioFile?.Dispose();

        outputDevice = null;
        audioFile = null;

        Console.WriteLine("Music stopped.");
    }
}
