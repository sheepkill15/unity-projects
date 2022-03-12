using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tarolo : MonoBehaviour
{
    public GameObject HousePanel;
    public GameObject dialog;
    public GameObject ShopPanel;
    public GameObject CarPanel;
    public Text residenttxt;
    public Text deathstxt;
    public Text stocktxt;
    public Text workerstxt;
    public GameObject Inv1;
    public GameObject Inv2;
    public GameObject Inv3;
    public GameObject Inv4;
    public GameObject Inv5;
    public GameObject Inv6;
    public GameObject money;
    public GameObject car;
    public string selected_name = "none";
    public int selected_stats = 0;
    public Image protection_txt;
    public GameObject Shop;
    public GameObject infection;
    public GameObject rep;
    public GameObject corna;

    public Sprite selected_image;

    public int workers_alive = -1;


    public Text kocsiName;
    public Text speed;
    public Text maxKaja;


    public GameObject[] panels;


    public void DisableAllPanels()
    {
        foreach(GameObject game in panels)
        {
            game.SetActive(false);
        }
    }


}
