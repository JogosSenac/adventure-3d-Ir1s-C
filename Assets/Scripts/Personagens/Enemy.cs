using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int vida = 100;
    [SerializeField] private float velocidade = 3f;
    [SerializeField] private float distanciaAtaque = 1.5f;
    [SerializeField] private float danoAtaque = 10f;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform jogador;
    private bool estaPerseguindo = false;
    private bool estaAtacando = false;
    private float distanciaParaJogador;

    private void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (jogador == null)
        {
            Debug.LogError("Jogador não encontrado! Verifique se a tag 'Player' está atribuída corretamente.");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator não encontrado no inimigo.");
        }
    }

    private void Update()
    {
        if (jogador == null || animator == null) return;

        distanciaParaJogador = Vector3.Distance(transform.position, jogador.position);

        PerseguirJogador();
        

        if (distanciaParaJogador <= distanciaAtaque && !estaAtacando)
        {
            Atacar();
        }
        else if (distanciaParaJogador > distanciaAtaque)
        {
            animator.SetBool("Atacando", false);
        }
    }

    public void EstaSeguindo()
    {
        estaPerseguindo = true;
        animator.SetBool("Andando", true);
    }

    public void NaoEstaSeguindo()
    {
        estaPerseguindo = false;
        animator.SetBool("Andando", false);
    }

    private void PerseguirJogador()
    {
        if (distanciaParaJogador > distanciaAtaque)
        {
            EstaSeguindo();
            animator.SetBool("Andando", true);
            Vector3 direcao = (jogador.position - transform.position).normalized;
            transform.position += direcao * velocidade * Time.deltaTime;
            LookAtJogador(direcao);
        }
        else
        {
            animator.SetBool("Andando", false);
            NaoEstaSeguindo();
        }
    }

    private void LookAtJogador(Vector3 direcao)
    {
        Quaternion rotacao = Quaternion.LookRotation(direcao);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacao, Time.deltaTime * 10f);
    }

    private void Atacar()
    {
        estaAtacando = true;
        animator.SetTrigger("Atacando");

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaAtaque))
        {
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<Player>().ReceberDano((int)danoAtaque);
            }
        }
        Invoke("ResetarAtaque", 1f);
    }

    private void ResetarAtaque()
    {
        estaAtacando = false;
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
        animator.SetTrigger("Morrer");
        Destroy(gameObject, 2f);
    }
}