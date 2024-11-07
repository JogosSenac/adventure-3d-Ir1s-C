using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip musicaJogo;
    public AudioClip musicaBoss;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicaJogo; // Música inicial 
        audioSource.Play();
    }

    public void MudarParaMusicaBoss()
    {
        audioSource.clip = musicaBoss;
        audioSource.Play();
    }
}
