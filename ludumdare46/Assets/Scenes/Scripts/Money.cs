using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{

    public int money=0;
    public Text money_txt;


    void Start()
    {
        money_txt.text = money.ToString();
    }

    public void STONKS( int i )
    {
        money += i;
        money_txt.text = money.ToString();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            STONKS(250);
        }
    }
}
