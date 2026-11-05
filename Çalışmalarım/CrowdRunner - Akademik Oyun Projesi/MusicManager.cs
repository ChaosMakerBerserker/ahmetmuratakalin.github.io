using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject); // Sahne deðiþince devam et
    }

    public void PlayMusic()
    {
        if (audioSource != null) audioSource.Play();
    }

    public void PauseMusic()
    {
        if (audioSource != null) audioSource.Pause();
    }
}