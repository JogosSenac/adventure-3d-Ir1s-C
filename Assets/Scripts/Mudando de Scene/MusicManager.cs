using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public AudioClip musicaDeFundo;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && musicaDeFundo != null)
        {
            audioSource.clip = musicaDeFundo;
            audioSource.loop = true; 
            audioSource.Play();
        }
    }
}
