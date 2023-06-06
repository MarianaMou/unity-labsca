using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enigme4 : MonoBehaviour
{
    public Light l4;
    public Button b1;
    public Button b2;
    public Button b3;
    public Button b4;
    public Button b5;
    public Button b6;
    public Button b7;
    public Button b8;
    public Button b9;
    private int p1 = 0;
    private int p2 = 0;
    private int p3 = 0;
    private int p4 = 0;
    private int p5 = 0;
    private int p6 = 0;
    private int p7 = 0;
    private int p8 = 0;
    private int p9 = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(p1==2 && p2==0 && p3==1 && p4==1 && p5==3 && p6 == 1 && p7 == 3 && p8 == 2 && p9 ==2){
l4.color = Color.green;
        }
    }

    public void changeColor1()
    {
        if (p1==0)
        {
            Debug.Log("vert");
            ColorBlock cb = b1.colors;
            cb.normalColor = Color.green;
            cb.highlightedColor = Color.green;
            cb.selectedColor = Color.green;
            b1.colors = cb;
            p1 = p1+1;
        }
        else if (p1==1)
        {
            Debug.Log("rouge");
            ColorBlock cb = b1.colors;
            cb.normalColor = Color.red;
            cb.highlightedColor = Color.red;
            cb.selectedColor = Color.red;
            b1.colors = cb;
            p1 = p1+1;
        }
        else if (p1==2)
        {
            Debug.Log("bleu");
            ColorBlock cb = b1.colors;
            cb.normalColor = Color.blue;
            cb.highlightedColor = Color.blue;
            cb.selectedColor = Color.blue;
            b1.colors = cb;
            p1 = p1+1;
        }
        else if (p1==3)
        {
            Debug.Log("blanc");
             ColorBlock cb = b1.colors;
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
            cb.selectedColor = Color.white;
            b1.colors = cb;
            p1 = 0;      
        } 
    }

    public void changeColor2()
    {
if (p2==0)
        {
            Debug.Log("vert");
            ColorBlock cb = b2.colors;
            cb.normalColor = Color.green;
            cb.highlightedColor = Color.green;
            cb.selectedColor = Color.green;
            b2.colors = cb;
            p2 = p2+1;
        }
        else if (p2==1)
        {
            Debug.Log("rouge");
            ColorBlock cb = b2.colors;
            cb.normalColor = Color.red;
            cb.highlightedColor = Color.red;
            cb.selectedColor = Color.red;
            b2.colors = cb;
            p2 = p2+1;
        }
        else if (p2==2)
        {
            Debug.Log("bleu");
            ColorBlock cb = b2.colors;
            cb.normalColor = Color.blue;
            cb.highlightedColor = Color.blue;
            cb.selectedColor = Color.blue;
            b2.colors = cb;
            p2 = p2+1;
        }
        else if (p2==3)
        {
            Debug.Log("blanc");
            ColorBlock cb = b2.colors;
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
            cb.selectedColor = Color.white;
            b2.colors = cb;
            p2 = 0;
            
        }
    }

       public void changeColor3()
    {
        if (p3==0)
        {
            Debug.Log("vert");
            ColorBlock cb = b3.colors;
            cb.normalColor = Color.green;
            cb.highlightedColor = Color.green;
            cb.selectedColor = Color.green;
            b3.colors = cb;
            p3 = p3+1;
        }
        else if (p3==1)
        {
            Debug.Log("rouge");
            ColorBlock cb = b3.colors;
            cb.normalColor = Color.red;
            cb.highlightedColor = Color.red;
            cb.selectedColor = Color.red;
            b3.colors = cb;
            p3 = p3+1;
        }
        else if (p3==2)
        {
            Debug.Log("bleu");
            ColorBlock cb = b3.colors;
            cb.normalColor = Color.blue;
            cb.highlightedColor = Color.blue;
            cb.selectedColor = Color.blue;
            b3.colors = cb;
            p3 = p3+1;
        }
        else if (p3==3)
        {
            Debug.Log("blanc");
            ColorBlock cb = b3.colors;
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
            cb.selectedColor = Color.white;
            b3.colors = cb;
            p3 = 0;      
        } 
    }

    public void changeColor4()
    {
        if (p4==0)
        {
            Debug.Log("vert");
            ColorBlock cb = b4.colors;
            cb.normalColor = Color.green;
            cb.highlightedColor = Color.green;
            cb.selectedColor = Color.green;
            b4.colors = cb;
            p4 = p4+1;
        }
        else if (p4==1)
        {
            Debug.Log("rouge");
            ColorBlock cb = b4.colors;
            cb.normalColor = Color.red;
            cb.highlightedColor = Color.red;
            cb.selectedColor = Color.red;
            b4.colors = cb;
            p4 = p4+1;
        }
        else if (p4==2)
        {
            Debug.Log("bleu");
            ColorBlock cb = b4.colors;
            cb.normalColor = Color.blue;
            cb.highlightedColor = Color.blue;
            cb.selectedColor = Color.blue;
            b4.colors = cb;
            p4 = p4+1;
        }
        else if (p4==3)
        {
            Debug.Log("blanc");
            ColorBlock cb = b4.colors;
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
            cb.selectedColor = Color.white;
            b4.colors = cb;
            p4 = 0;      
        } 
    }

     public void changeColor5()
    {
        if (p5==0)
        {
            Debug.Log("vert");
            ColorBlock cb = b5.colors;
            cb.normalColor = Color.green;
            cb.highlightedColor = Color.green;
            cb.selectedColor = Color.green;
            b5.colors = cb;
            p5 = p5+1;
        }
        else if (p5==1)
        {
            Debug.Log("rouge");
            ColorBlock cb = b5.colors;
            cb.normalColor = Color.red;
            cb.highlightedColor = Color.red;
            cb.selectedColor = Color.red;
            b5.colors = cb;
            p5 = p5+1;
        }
        else if (p5==2)
        {
            Debug.Log("bleu");
            ColorBlock cb = b5.colors;
            cb.normalColor = Color.blue;
            cb.highlightedColor = Color.blue;
            cb.selectedColor = Color.blue;
            b5.colors = cb;
            p5 = p5+1;
        }
        else if (p5==3)
        {
            Debug.Log("blanc");
            ColorBlock cb = b5.colors;
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
            cb.selectedColor = Color.white;
            b5.colors = cb;
            p5 = 0;      
        } 
    }

     public void changeColor6()
    {
        if (p6==0)
        {
            Debug.Log("vert");
            ColorBlock cb = b6.colors;
            cb.normalColor = Color.green;
            cb.highlightedColor = Color.green;
            cb.selectedColor = Color.green;
            b6.colors = cb;
            p6 = p6+1;
        }
        else if (p6==1)
        {
            Debug.Log("rouge");
            ColorBlock cb = b6.colors;
            cb.normalColor = Color.red;
            cb.highlightedColor = Color.red;
            cb.selectedColor = Color.red;
            b6.colors = cb;
            p6 = p6+1;
        }
        else if (p6==2)
        {
            Debug.Log("bleu");
            ColorBlock cb = b6.colors;
            cb.normalColor = Color.blue;
            cb.highlightedColor = Color.blue;
            cb.selectedColor = Color.blue;
            b6.colors = cb;
            p6 = p6+1;
        }
        else if (p6==3)
        {
            Debug.Log("blanc");
            ColorBlock cb = b6.colors;
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
            cb.selectedColor = Color.white;
            b6.colors = cb;
            p6 = 0;      
        } 
    }

     public void changeColor7()
    {
        if (p7==0)
        {
            Debug.Log("vert");
            ColorBlock cb = b7.colors;
            cb.normalColor = Color.green;
            cb.highlightedColor = Color.green;
            cb.selectedColor = Color.green;
            b7.colors = cb;
            p7 = p7+1;
        }
        else if (p7==1)
        {
            Debug.Log("rouge");
            ColorBlock cb = b7.colors;
            cb.normalColor = Color.red;
            cb.highlightedColor = Color.red;
            cb.selectedColor = Color.red;
            b7.colors = cb;
            p7 = p7+1;
        }
        else if (p7==2)
        {
            Debug.Log("bleu");
            ColorBlock cb = b7.colors;
            cb.normalColor = Color.blue;
            cb.highlightedColor = Color.blue;
            cb.selectedColor = Color.blue;
            b7.colors = cb;
            p7 = p7+1;
        }
        else if (p7==3)
        {
            Debug.Log("blanc");
            ColorBlock cb = b7.colors;
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
            cb.selectedColor = Color.white;
            b7.colors = cb;
            p7 = 0;      
        } 
    }

    public void changeColor8()
    {
        if (p8==0)
        {
            Debug.Log("vert");
            ColorBlock cb = b8.colors;
            cb.normalColor = Color.green;
            cb.highlightedColor = Color.green;
            cb.selectedColor = Color.green;
            b8.colors = cb;
            p8 = p8+1;
        }
        else if (p8==1)
        {
            Debug.Log("rouge");
            ColorBlock cb = b8.colors;
            cb.normalColor = Color.red;
            cb.highlightedColor = Color.red;
            cb.selectedColor = Color.red;
            b8.colors = cb;
            p8 = p8+1;
        }
        else if (p8==2)
        {
            Debug.Log("bleu");
            ColorBlock cb = b8.colors;
            cb.normalColor = Color.blue;
            cb.highlightedColor = Color.blue;
            cb.selectedColor = Color.blue;
            b8.colors = cb;
            p8 = p8+1;
        }
        else if (p8==3)
        {
            Debug.Log("blanc");
            ColorBlock cb = b8.colors;
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
            cb.selectedColor = Color.white;
            b8.colors = cb;
            p8 = 0;      
        } 
    }

    public void changeColor9()
    {
        if (p9==0)
        {
            Debug.Log("vert");
            ColorBlock cb = b9.colors;
            cb.normalColor = Color.green;
            cb.highlightedColor = Color.green;
            cb.selectedColor = Color.green;
            b9.colors = cb;
            p9 = p9+1;
        }
        else if (p9==1)
        {
            Debug.Log("rouge");
            ColorBlock cb = b9.colors;
            cb.normalColor = Color.red;
            cb.highlightedColor = Color.red;
            cb.selectedColor = Color.red;
            b9.colors = cb;
            p9 = p9+1;
        }
        else if (p9==2)
        {
            Debug.Log("bleu");
            ColorBlock cb = b9.colors;
            cb.normalColor = Color.blue;
            cb.highlightedColor = Color.blue;
            cb.selectedColor = Color.blue;
            b9.colors = cb;
            p9 = p9+1;
        }
        else if (p9==3)
        {
            Debug.Log("blanc");
            ColorBlock cb = b9.colors;
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
            cb.selectedColor = Color.white;
            b9.colors = cb;
            p9 = 0;      
        } 
    }
}
