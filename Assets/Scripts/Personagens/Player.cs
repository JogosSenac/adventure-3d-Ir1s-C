using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int vida = 100;
    [SerializeField] private int ataque;
    [SerializeField] private float velocidade;
    [SerializeField] private int forcaPulo;
    [SerializeField] private int ouro;
    [SerializeField] private bool temChave;
    [SerializeField] private bool pegando;
    [SerializeField] private bool podePegar;
    [SerializeField] private Animator animator;
    [SerializeField] private bool estaVivo = true;
    [SerializeField] private float distanciaAtaque = 1.5f;
    [SerializeField] private float danoAtaque = 10f;
    [SerializeField] private List<GameObject> inventario = new List<GameObject>();
    private bool estaAtacando = false;
    private Rigidbody rb;
    private bool estaPulando;
    private Vector3 angleRotation;

    void Start()
    {
        temChave = false;
        pegando = false;
        podePegar = false;
        angleRotation = new Vector3(0, 90, 0);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!estaVivo) return;

        if (Input.GetKeyDown(KeyCode.E) && podePegar)
        {
            animator.SetTrigger("Pegando");
            pegando = true;
        }
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("Andar", true);
            animator.SetBool("AndarParaTras", false);
            Walk();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("AndarParaTras", true);
            animator.SetBool("Andar", false);
            Walk();
        }
        else
        {
            animator.SetBool("Andar", false);
            animator.SetBool("AndarParaTras", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !estaPulando)
        {
            animator.SetTrigger("Pular");
            Jump();
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Ataque");
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("Correndo", true);
            Walk(8);
        }
        else
        {
            animator.SetBool("Correndo", false);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("Defesa");
        }

        TurnAround();
    }

    private void Walk(float velo = 1)
    {
        if (velo == 1)
        {
            velo = velocidade;
        }
        float fowardInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * fowardInput;
        Vector3 moveFoward = rb.position + moveDirection * velo * Time.deltaTime;
        rb.MovePosition(moveFoward);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * forcaPulo, ForceMode.Impulse);
        estaPulando = true;
        animator.SetBool("EstaNoChao", false);
    }

    private void TurnAround()
    {
        float sideInput = Input.GetAxis("Horizontal");
        Quaternion deltaRotation = Quaternion.Euler(angleRotation * sideInput * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            estaPulando = false;
            animator.SetBool("EstaNoChao", true);
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<BossEnemy>().ReceberDano(Atacar());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            podePegar = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            podePegar = false;
        }
    }

    public void ReceberDano(int quantidade)
    {
        vida -= quantidade;
        if (vida <= 0 && estaVivo)
        {
            estaVivo = false;
            animator.SetTrigger("Morte");
            gameObject.SetActive(false);
        }
    }

    private int Atacar()
    {
        estaAtacando = true;
        animator.SetTrigger("Atacando");
        Invoke("ResetarAtaque", 1f);
        return ataque;
    }
}
