using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

   public class Player : MonoBehaviour
   {
      [SerializeField] private Animator animator;
      [SerializeField] private bool estaVivo = true;
      [SerializeField] private int ouro;
      [SerializeField] private int vida;
      [SerializeField] private int forcaPulo;
      [SerializeField] private float velocidade;
      [SerializeField] private bool temChave; 
      [SerializeField] private List<GameObject> inventario = new List<GameObject>();
      private Rigidbody rb;
      private bool estaPulando;
      private Vector3 angleRotation;
      private bool pegando;
      private bool podePegar;

      void Start()
      {
         temChave = false;
         pegando = false;
         angleRotation = new Vector3(0, 90, 0);
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
         else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
         {
            animator.SetBool("Andar", true);
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
         Walk(14);
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
      
         TurnAround();

      }


      private void Walk(float velo = 1)
      {
         if((velo == 1))
         {
            velo = velocidade;
         }
         float fowardInput = Input.GetAxis("Vertical");
         Vector3 moveDirection = transform.forward * fowardInput;
         Vector3 moveFoward = rb.position + moveDirection * velo *Time.deltaTime;
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
         if(collision.gameObject.CompareTag("Chao"))
         {
            estaPulando = false;
            animator.SetBool("EstaNoChao", true);
         }
      }

      private void OnTriggerStay(Collider other)
      {
         Debug.Log(other.gameObject.tag);

         if(Input.GetKeyDown(KeyCode.E))
         {
            animator.SetTrigger("Pegando");
            pegando = true;
         }
         if(other.gameObject.CompareTag("Chave") && pegando)
         {
            inventario.Add(Instantiate(other.gameObject.GetComponent<Chave>().CopiaDaChave()));
            int numero = other.gameObject.GetComponent<Chave>().PegarNumeroChave();
            Debug.LogFormat($"Chave número: {numero} foi inserida no invenário", numero);
            Destroy(other.gameObject);
            podePegar = false;
            pegando = false;
         }
         if(other.gameObject.CompareTag("Porta") && pegando && temChave)
         {
            other.gameObject.GetComponent<Animator>().SetTrigger("Abrir");
            temChave = false;
         }
         if(other.gameObject.CompareTag("Bau") && pegando)
         {
            if(VerificaChave(other.gameObject.GetComponent<Bau>().PegarNumeroFechadura()))
            {
               other.gameObject.GetComponent<Animator>().SetTrigger("Abrir");
            }
            else 
            {
               Debug.Log("Você não possui a Chave");
            }
            
         }

         pegando = false;

      }

      private void OnCollisionExit(Collision other)
      {
         pegando = false;
      }

      private bool VerificaChave(int chave)
      {
         foreach(GameObject item in inventario)
         {
            if(item.gameObject.CompareTag("Chave"))
            {
               if(item.gameObject.GetComponent<Chave>().PegarNumeroChave() == chave)
               {
                  return true;
               }
            }
         }

         return false;
      }

      private void PegarConteudoBau(GameObject bau)
      {
         Bau bauTesouro = bau.GetComponent<Bau>();

         ouro = bauTesouro.gameObject.GetComponent<Bau>().PegarOuro();

         if(bauTesouro.GetComponent<Bau>().AcessarConteudoBau() != null)
         {
            foreach(GameObject Item in bauTesouro.GetComponent<Bau>().AcessarConteudoBau())
            {
               inventario.Add(Item);
            }
         }
      }

   }