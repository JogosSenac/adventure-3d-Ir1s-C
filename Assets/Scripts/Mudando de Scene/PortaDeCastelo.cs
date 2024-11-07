using UnityEngine;
using UnityEngine.SceneManagement;

public class PortaDeCastelo : MonoBehaviour
{
    public string nomeCenaBoss = "Boss";
    public AudioManager audioManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioManager != null)
            {
                audioManager.MudarParaMusicaBoss();
            }
            SceneManager.LoadScene(nomeCenaBoss);
        }
    }
}
