using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sortie : MonoBehaviour
{
    public Light light1;
    public Light light2;
    public Light light3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (light1.color == Color.green && light2.color == Color.green && light3.color == Color.green) {
           Debug.Log("Tu peux sortir");
           Destroy(gameObject);
        }
    }
}
