using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TransicaoCena : MonoBehaviour
{
    public Image telaPreta;
    public float tempoFade = 1f;

    public void MudarCenaComFade(string nomeCena)
    {
        StartCoroutine(FadeECarregarCena(nomeCena));
    }

    private IEnumerator FadeECarregarCena(string nomeCena)
    {
        float tempo = 0;
        while (tempo < tempoFade)
        {
            tempo += Time.deltaTime;
            telaPreta.color = Color.Lerp(Color.clear, Color.black, tempo / tempoFade);
            yield return null;
        }

        SceneManager.LoadScene(nomeCena);

        tempo = 0;
        while (tempo < tempoFade)
        {
            tempo += Time.deltaTime;
            telaPreta.color = Color.Lerp(Color.black, Color.clear, tempo / tempoFade);
            yield return null;
        }
    }
}
