using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool estaVivo = true;
    [SerializeField] private int forcaPulo;
    [SerializeField] private float velocidade;
    private Rigidbody rb;
    private bool estaPulando;
    private bool mover;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
     //Andar
        if(Input.GetKey(KeyCode.W))
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

     //Evitar Bugde Movimentação
     if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
     {
          animator.SetBool("Andar", false);
          animator.SetBool("AndarParaTras", false);
     }

     //Pular
       if(Input.GetKeyDown(KeyCode.Space) && !estaPulando)
       {
          animator.SetTrigger("Pular");
          Jump();
       }

     //Pegando
       if(Input.GetKeyDown(KeyCode.E))
       {
          animator.SetTrigger("Pegando");
       }

       //Ataque
       if(Input.GetMouseButtonDown(0))
       {
          animator.SetTrigger("Ataque");
       }

       //Correndo
       if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
       {
          animator.SetBool("Correndo", true);
          Walk(8);
       }
       else
       {
          animator.SetBool("Correndo", false);
       }

     //Vida
       if(!estaVivo)
       {
          animator.SetTrigger("EstaVivo");
          estaVivo=true;
       }

    }

    private void Walk(float velo = 1)
    {
          if((velo == 1))
          {
               velo = velocidade;
          }
          float moveV = Input.GetAxis("Vertical");
          transform.position += new Vector3(0, 0, moveV * velocidade * Time.deltaTime);
    }

    private void Jump()
    {
          rb.AddForce(Vector3.up * forcaPulo, ForceMode.Impulse);
          estaPulando = true;
          animator.SetBool("EstaNoChao", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
          if(collision.gameObject.CompareTag("Chão"))
          {
               estaPulando = false;
               animator.SetBool("EstaNoChao", true);
          }
    }
    
}
