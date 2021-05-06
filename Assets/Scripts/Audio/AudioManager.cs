using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] audioTracks;
    public int FirstTrack;
    public bool AutoPlay;
    private int currentTrack;

    private static AudioManager sharedInstance = null;

    public static AudioManager SharedInstance
    {
        get
        {
            return sharedInstance;
        }
    }

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        sharedInstance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        currentTrack = FirstTrack;
        if(AutoPlay) audioTracks[FirstTrack].Play();
    }

    public void PlayCurrentTrack()
    {
        if (!audioTracks[currentTrack].isPlaying)
        {
            audioTracks[currentTrack].Play();
        }
    }

    public void Stop()
    {
        audioTracks[currentTrack].Stop();
    }

    public void PlayNewTrack(int newTrack)
    {
        audioTracks[currentTrack].Stop();
        currentTrack = newTrack;
        audioTracks[currentTrack].Play();
    }
}
