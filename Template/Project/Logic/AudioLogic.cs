using NAudio.Wave;

public static class AudioLogic
{
    public static string CurrentGenre = "Action";

    private static WaveOutEvent bgOutput;
    private static WaveFileReader bgReader;

    public static void PlayMoveSound()
    {
        PlaySound($"Logic/MusicMove/{CurrentGenre}Move.wav");
    }

    public static void PlayReservationSuccessSound()
    {
        PlaySound($"Logic/MusicSucRes/{CurrentGenre}SucRes.wav");
    }

    public static void PlayBackgroundMusic()
    {
        StopBackgroundMusic();

        bgReader = new WaveFileReader($"Logic/MusicBG/{CurrentGenre}BG.wav");
        bgOutput = new WaveOutEvent();
        bgOutput.Init(bgReader);
        bgOutput.Play();
    }

    public static void StopBackgroundMusic()
    {
        bgOutput?.Stop();
        bgOutput?.Dispose();
        bgReader?.Dispose();

        bgOutput = null;
        bgReader = null;
    }

    private static void PlaySound(string path)
    {
        var reader = new WaveFileReader(path);
        var output = new WaveOutEvent();
        output.Init(reader);
        output.Play();

        output.PlaybackStopped += (s, e) =>
        {
            output.Dispose();
            reader.Dispose();
        };
    }
}
