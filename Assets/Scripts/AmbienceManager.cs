using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Drag your 6 audio clips here")]
    public AudioClip[] songs;

    [Range(0f, 1f)]
    public float volume = 0.5f;

    private AudioSource audioSource;
    private int currentSongIndex = 0;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = volume;
    }

    void Start()
    {
        if (songs.Length > 0)
        {
            PlayRandomSong();
        }
    }

    public void PlayRandomSong()
    {
        currentSongIndex = Random.Range(0, songs.Length);
        PlaySongByIndex(currentSongIndex);
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        audioSource.volume = volume;
    }

    public void PlayNextSong()
    {
        currentSongIndex = (currentSongIndex + 1) % songs.Length;
        PlaySongByIndex(currentSongIndex);
    }

    public void PlaySongByIndex(int index)
    {
        if (index < 0 || index >= songs.Length)
        {
            Debug.LogError("AmbienceManager: Invalid song index requested.");
            return;
        }

        currentSongIndex = index;

        audioSource.clip = songs[index];
        audioSource.Play();

        Debug.Log($"Playing Ambience: {songs[index].name}");
    }
}