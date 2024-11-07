using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject inimigoPrefab; 
    public int quantidadeInimigos = 5; 
    public Vector2 limiteX = new Vector2(-50f, 50f); 
    public Vector2 limiteZ = new Vector2(-50f, 50f); 
    public float alturaSpawn = 1f; 

    void Start()
    {
        SpawnInimigos();
    }

    void SpawnInimigos()
    {
        for (int i = 0; i < quantidadeInimigos; i++)
        {
            Vector3 posicaoAleatoria = new Vector3(Random.Range(limiteX.x, limiteX.y),alturaSpawn,Random.Range(limiteZ.x, limiteZ.y));
            Instantiate(inimigoPrefab, posicaoAleatoria, Quaternion.identity);
        }
    }

    // Desenha os limites de spawn no Editor para visualização
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3((limiteX.x + limiteX.y) / 2, alturaSpawn, (limiteZ.x + limiteZ.y) / 2),new Vector3(limiteX.y - limiteX.x, 0.1f, limiteZ.y - limiteZ.x));
    }
}
