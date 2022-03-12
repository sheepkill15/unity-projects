using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reputation : MonoBehaviour
{

    public float rep = 100.1f;
    public Text rep_txt;

    void Start()
    {
        rep_txt.text = ((int)(rep)).ToString();
    }



    public void change(float i)
    {
        if (rep + i < 0)
            rep = 0;
        else
            rep += i;


        UpdateUI();
    }


    public void UpdateUI()
    {
        rep_txt.text = ((int)(rep)).ToString();
    }


 void Update()
 {
     if (Input.GetKeyDown("g"))
         rep+=10;
 }
}
