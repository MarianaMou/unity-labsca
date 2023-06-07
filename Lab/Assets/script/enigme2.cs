using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enigme2 : MonoBehaviour
{
    private bool p2 = false;
    public Light l2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (p2) {
            l2.color = Color.green;
        }  
    }
    
      public void strrrrrr(string str) 
    {
        Debug.Log(str);
        if (str == "modal")
        {
            //Debug.Log("bien ouej");
            p2 = true;
        }
        else
        {
            //Debug.Log(" pas bien ouej");
            p2 = false;
        }
    }
}
