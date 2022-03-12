using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buything : MonoBehaviour
{
    MoveCar cuccok;
    GameObject amount;
    Money money;


    public GameObject mitvesz;

    public int cost;

    private void Start()
    {
        amount = GameObject.Find("Canvas");
        money = amount.GetComponent<Tarolo>().money.GetComponent<Money>();
        cuccok = GameObject.Find("GameManager").GetComponent<MoveCar>();
    }

    public void Buy()
    {
        if(money.money >= cost)
        {
            money.STONKS(-cost);
            cuccok.AddCar(mitvesz);
        }
    }
}
