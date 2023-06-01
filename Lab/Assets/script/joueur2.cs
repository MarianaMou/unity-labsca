using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joueur2 : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotaX = 54f;
    public float rotaY = 0f;
    public float maxRotaX;
    public float maxRotaY;
    public float minRotaX;
    public float minRotaY;

    public float sensi = 5f;
    void Start()
    {
       // Debug.Log("X" + transform.rotation.x + "Y" + transform.rotation.y);
    }

    // Update is called once per frame
    void Update()
    {
        rotaX += Input.GetAxis("Mouse X") * sensi;
        rotaY += Input.GetAxis("Mouse Y") * -sensi;

        //Y 55à 15
        if (rotaY > maxRotaY)
        {
            rotaY = maxRotaY;
        }
        if (rotaY < minRotaY)
        {
            rotaY = minRotaY;
        }
        
       
        //transform.localEulerAngles = new Vector3(rotaY, rotaX, 0f);
    }
}
