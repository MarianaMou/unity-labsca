using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enigme3 : MonoBehaviour
{
    private int slider;
    private int slider1;
    private int slider2;
    public TMPro.TMP_Text ValueSlider;
    public TMPro.TMP_Text ValueSlider1;
    public TMPro.TMP_Text ValueSlider2;
    private bool p1 = false;
    private bool p2 = false;
    private bool p3 = false;
    public Light l3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (p1&&p2&&p3) {
            l3.color = Color.green;
        }
    }

    public void SliderInput(float f)
    {
        slider = (int)(f * 100);
        Debug.Log("le slider" + slider);
        ValueSlider.text = slider.ToString();
        if (slider == 37)
        {
            Debug.Log("ouais ouais");
            p3 = true;
        }
        else
        {
            p3 = false;
        }
    }

    public void SliderInput1(float f)
    {
        slider1 = (int)(f * 100);
        Debug.Log("le slider1" + slider1);
        ValueSlider1.text = slider1.ToString();
        if (slider1 == 68)
        {
            Debug.Log("ouais ouais");
            p2 = true;
        }
        else
        {
            p2 = false;
        }
    }

       public void SliderInput2(float f)
    {
        slider2 = (int)(f * 100);
        Debug.Log("le slider2" + slider2);
        ValueSlider2.text = slider2.ToString();
        if (slider2 == 11)
        {
            Debug.Log("ouais ouais");
            p1 = true;
        }
        else
        {
            p1 = false;
        }
    }
}
