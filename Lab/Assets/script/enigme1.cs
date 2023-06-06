using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enigme1 : MonoBehaviour
{
    private string input;
    private int slider;
    public TMPro.TMP_Text ValueSlider;
    private bool p1 = false;
    private bool p2 = false;
    public Light l1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (p1&&p2) {
            l1.color = Color.green;
        }

        
    }

    public void SliderInput(float f)
    {
        slider = (int)(f * 100);
        Debug.Log("le lsider" + slider);
        ValueSlider.text = slider.ToString();
        if (slider == 37)
        {
            Debug.Log("ouais ouais");
            p1 = true;
        }
        else
        {
            p1 = false;
        }
    }

    public void readStringInput(string s) 
    {
        input = s;
        Debug.Log(input);
        if (input == "bravo")
        {
            Debug.Log("bien ouej");
            p2 = true;
        }
        else
        {
            p2 = false;
        }
    }

   
}
