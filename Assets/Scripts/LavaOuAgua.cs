using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaOuAgua : MonoBehaviour
{
    public float speedY = 0.05f;
    public float speedX = 0.05f;
    MeshRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
       rend = GetComponent<MeshRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        rend.material.mainTextureOffset = new Vector2(speedX * Time.timeSinceLevelLoad, speedY * Time.timeSinceLevelLoad);
    }
}