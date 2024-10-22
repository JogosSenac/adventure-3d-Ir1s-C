using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bau : MonoBehaviour
{
    [SerializeField] GameObject particulas;
    [SerializeField] private bool ehMagico;
    [SerializeField] private int numeroChave;
    [SerializeField] private List<GameObject>itens = new List<GameObject>();
    [SerializeField] private int ouro; 

    void Start()
    {
        if(ehMagico)
        {
            particulas.SetActive(true);
            ouro = Random.Range(10,50);
        }
        else
        {
            particulas.SetActive(false);
            ouro = Random.Range(0,10);
        }
    }

    private void DesativarParticulas()
    {
        //particulas.SetActive(false); -->some repentinamente
        particulas.GetComponent<ParticleSystem>().Stop(); //some devagar
    }

    public int PegarOuro()
    {
        DesativarParticulas();
        StartCoroutine(ZerarBau());
        return ouro;
    }

    IEnumerator  ZerarBau()
    {
        yield return new WaitForSeconds(2.5f);
        ouro = 0;
    }

    public int PegarNumeroFechadura()
    {
        return numeroChave;
    }

    public List<GameObject> AcessarConteudoBau()
    {
        return itens;
    }

    public void RemoverConteudoBau()
    {
        itens.Clear();
    }

}