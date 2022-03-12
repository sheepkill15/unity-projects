using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoronaShop : MonoBehaviour
{
    public GameObject shop;
    public GameObject tarolo;
    public GameObject bank;
    public int money;
    private MoveCar osszesKocsi;
    public GameObject check;

    private void Start()
    {
        osszesKocsi = GameObject.Find("GameManager").GetComponent<MoveCar>();
    }

    public void checkshow()
    {
        check.SetActive(true);
    }

    public void chekhide()
    {
        check.SetActive(false);
    }


    public void Close ()
    {
        bool ja = false;
        if (!shop.activeSelf)
            ja = true;
        tarolo.GetComponent<Tarolo>().DisableAllPanels();
        shop.SetActive(ja);
        Camera.main.GetComponent<CameraControl>().canmove = !ja;
    }

    public void Check()
    {
        money = bank.GetComponent<Money>().money;
        if (money >= 200)
        {
            tarolo.GetComponent<Tarolo>().selected_name = "Check";
            bank.GetComponent<Money>().STONKS(-200);
            check.SetActive(true);
        }
    }

    public void WeakMdicine()
    {
        money = bank.GetComponent<Money>().money;
        if (money >= 350)
        {
            tarolo.GetComponent<Tarolo>().selected_name = "Med";
            tarolo.GetComponent<Tarolo>().selected_stats = 45;
            bank.GetComponent<Money>().STONKS(-350);
            check.SetActive(true);
        }
    }

    public void MeadiumMdicine()
    {
        money = bank.GetComponent<Money>().money;
        if (money >= 550)
        {
            tarolo.GetComponent<Tarolo>().selected_name = "Med";
            tarolo.GetComponent<Tarolo>().selected_stats = 65;
            bank.GetComponent<Money>().STONKS(-550);
            check.SetActive(true);
        }
    }

    public void StrongMdicine()
    {
        money = bank.GetComponent<Money>().money;
        if (money >= 850)
        {
            tarolo.GetComponent<Tarolo>().selected_name = "Med";
            tarolo.GetComponent<Tarolo>().selected_stats = 85;
            bank.GetComponent<Money>().STONKS(-850);
            check.SetActive(true);
        }
    }


    public void CheckAll()
    {
        money = bank.GetComponent<Money>().money;
        if (money >= 1500)
        {
                //IDEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
            foreach(KocsiMozogj car in osszesKocsi.cars)
            {
                car.GetComponent<Inventory>().checkall();
            }
                //IDEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
            bank.GetComponent<Money>().STONKS(-1500);
        }
    }
}
