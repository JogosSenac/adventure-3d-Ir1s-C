using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MenuController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject menuOpcoes;
    public GameObject rawImage;
    public Animator animatorRawImage;
    public AudioSource backgroundMusic;
    public AudioSource buttonClickSound;
    public AudioSource mouseClickSound;

    private bool hasClicked = false;

    void Start()
    {
        animatorRawImage = rawImage.GetComponent<Animator>();
        rawImage.SetActive(false);
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play(); 
        }
        DontDestroyOnLoad(backgroundMusic.gameObject); 
        menuOpcoes.SetActive(false); 
    }

    void Update()
    {
        if (!videoPlayer.isPlaying && !hasClicked)
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0)) 
            {
                if (Input.GetMouseButtonDown(0) && mouseClickSound != null)  
                {
                    mouseClickSound.Play(); 
                }
                videoPlayer.Play();
                rawImage.SetActive(true);
                animatorRawImage.SetTrigger("FadeIn");
                menuOpcoes.SetActive(true);  
                hasClicked = true; 
            }
        }
    }

    public void OnPlayButtonPressed()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }

        SceneManager.LoadScene("Jogo");
    }
}
