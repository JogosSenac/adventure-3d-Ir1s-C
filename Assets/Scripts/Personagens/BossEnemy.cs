using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] private float vida = 200;
    [SerializeField] private float ataque = 20f;
    [SerializeField] private float velocidade = 3f;
    [SerializeField] private float distanciaDeAtaque = 2f;
    [SerializeField] private float distanciaDePerseguicao = 10f;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform jogador;
    private bool estaPerseguindo = false;
    private bool podeAtacar = true;

    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distancia = Vector3.Distance(transform.position, jogador.position);

        if (distancia <= distanciaDePerseguicao && distancia > distanciaDeAtaque)
        {
            SeguirJogador(distancia);
        }
        else if (distancia <= distanciaDeAtaque)
        {
            AtacarJogador();
        }
        else
        {
            PararPerseguicao();
        }
    }

    private void SeguirJogador(float distancia)
    {
        if (!estaPerseguindo)
        {
            estaPerseguindo = true;
            animator.SetBool("Andando", true);
        }

        Vector3 direcao = (jogador.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, jogador.position, velocidade * Time.deltaTime);
        Quaternion rotacao = Quaternion.LookRotation(direcao);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacao, Time.deltaTime * 5f);
    }

    private void AtacarJogador()
    {
        if (animator != null)
        {
            animator.SetTrigger("Ataque");
        }

        if (Vector3.Distance(transform.position, jogador.position) <= distanciaDeAtaque && podeAtacar)
        {
            Invoke("CausarDanoPlayer", 3.0f);
            podeAtacar = false;
        }
    }

    private void CausarDanoPlayer()
    {
        jogador.GetComponent<Player>().ReceberDano((int)ataque);
        podeAtacar = true;
    }

    private void PararPerseguicao()
    {
        if (estaPerseguindo)
        {
            estaPerseguindo = false;
            animator.SetBool("Perseguindo", false);
        }
    }

    public void ReceberDano(int quantidade)
    {
        vida -= quantidade;
        if (vida <= 0)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        animator.SetTrigger("Morte");
        gameObject.SetActive(false);
    }
}

