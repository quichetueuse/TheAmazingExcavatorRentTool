using System.Windows.Media;

namespace TheAmazingExcavatorRentTool.Services;

public class SoundPlayer
{
    private MediaPlayer soundPlayer;

    public bool isRunning;

    public Uri successSoundPath;

    public Uri failSoundPath;
    public SoundPlayer()
    {
        // Creating media player
        soundPlayer = new MediaPlayer();
        soundPlayer.MediaOpened += OnMediaOpened;
        soundPlayer.MediaFailed += OnMediaFailed;
        soundPlayer.MediaEnded += OnMediaEnded;

        successSoundPath = new Uri(@"C:\Users\Eliot\RiderProjects\TheAmazingExcavatorRentTool\TheAmazingExcavatorRentTool\Assets\applepay.mp3");

        isRunning = false;
    }

    // public void PlayAudio(string audio_url)
    // {
    //     Uri uri = new Uri(audio_url);
    //     soundPlayer.Open(uri);
    // }

    public void PlaySuccessSound()
    {
        if (isRunning)
            return;
        soundPlayer.Open(successSoundPath);
    }
    
    
    private void OnMediaOpened(object sender, EventArgs e)
    {
        isRunning = true;
        var player = (MediaPlayer)sender;
        player.Play();
        Console.Write("Successfully played sound!");
    }

    private void OnMediaFailed(object sender, ExceptionEventArgs e)
    {
        var exception = e.ErrorException;
        // Handle exception
    }

    private void OnMediaEnded(object sender, EventArgs e)
    {
        isRunning = false;
    }
}