using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public GameObject bank;
    public int money;
    public GameObject tarolo;
    public GameObject shop;
    public GameObject help_window;
    public GameObject Stop;

    public void Help()
    {
        help_window.SetActive(true);
    }

    public void HelpClose()
    {
        help_window.SetActive(false);
    }

    public void HandSan (Image image)
    {
        money = bank.GetComponent<Money>().money;

        if ( money > 125 )
        {

        bank.GetComponent<Money>().STONKS(-125);

            tarolo.GetComponent<Tarolo>().selected_name = "Hand Sanitizer";
            tarolo.GetComponent<Tarolo>().selected_stats = 15;
            tarolo.GetComponent<Tarolo>().selected_image = image.sprite;
            Help();
        }
    }

    public void Mask(Image image)
    {
        money = bank.GetComponent<Money>().money;

        if (money >= 200)
        {

            bank.GetComponent<Money>().STONKS(-200);

            tarolo.GetComponent<Tarolo>().selected_name = "Mask";
            tarolo.GetComponent<Tarolo>().selected_stats = 25;
            tarolo.GetComponent<Tarolo>().selected_image = image.sprite;
            Help();
        }
    }

    public void MasknGloves(Image image)
    {
        money = bank.GetComponent<Money>().money;

        if (money >= 250)
        {

            bank.GetComponent<Money>().STONKS(-250);

            tarolo.GetComponent<Tarolo>().selected_name = "Mask & Gloves";
            tarolo.GetComponent<Tarolo>().selected_stats = 45;
            tarolo.GetComponent<Tarolo>().selected_image = image.sprite;
            Help();
        }
    }

    public void GasMask(Image image)
    {
        money = bank.GetComponent<Money>().money;

        if (money >= 400)
        {

            bank.GetComponent<Money>().STONKS(-400);

            tarolo.GetComponent<Tarolo>().selected_name = "Gas Mask";
            tarolo.GetComponent<Tarolo>().selected_stats = 80;
            tarolo.GetComponent<Tarolo>().selected_image = image.sprite;
            Help();
        }
    }


    public void HazardSuit(Image image)
    {
        money = bank.GetComponent<Money>().money;

        if (money >= 650)
        {

            bank.GetComponent<Money>().STONKS(-650);

            tarolo.GetComponent<Tarolo>().selected_name = "Hazard Suit";
            tarolo.GetComponent<Tarolo>().selected_stats = 95;
            tarolo.GetComponent<Tarolo>().selected_image = image.sprite;
            Help();
        }
    }


    public void close()
    {
        bool ja = false;
        if (!shop.activeSelf)
            ja = true;
        tarolo.GetComponent<Tarolo>().DisableAllPanels();
        shop.SetActive(ja);
        Stop.GetComponent<CameraControl>().canmove = !ja;
    }
}
