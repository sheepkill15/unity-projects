using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Pathfinding;

public class CheckClickUIGOmb : MonoBehaviour, IPointerEnterHandler ,IPointerExitHandler
{
    public GameObject panel;
    public GameObject canvas;
    public GameObject car;
    private Inventory carInventory;
    public GameObject shop;
    public GameObject shop2;
    private KocsiMozogj kocsi;


    void Start()
    {
        canvas = GameObject.Find("Canvas");
        panel = canvas.GetComponent<Tarolo>().CarPanel;
        carInventory = car.GetComponent<Inventory>();
        shop = canvas.GetComponent<Tarolo>().Shop;
        shop2 = canvas.GetComponent<Tarolo>().corna;
        kocsi = carInventory.GetComponent<KocsiMozogj>();
        Color color = kocsi.color;
        color.a = 1;
        GetComponent<Image>().color = color;
    }


    void Update()
    {
        if(kocsi.chosen)
        {
            Color color = kocsi.color;
            color.a = 1f;
            GetComponent<Image>().color = color;
        }
        else
        {
            Color color = kocsi.color;
            color.a = 0.25f;
            GetComponent<Image>().color = color;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        carInventory.highLighted = true;
        panel.SetActive(true);
        carInventory.UpdateUI();
        canvas.GetComponent<Tarolo>().kocsiName.text = carInventory.kocsiname;
        canvas.GetComponent<Tarolo>().speed.text = carInventory.gameObject.GetComponent<AIPath>().maxSpeed.ToString();
        canvas.GetComponent<Tarolo>().maxKaja.text = carInventory.max.ToString();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
            carInventory.highLighted = false;
            panel.SetActive(false);
    }



    public void notOnMouseDown()
    {
        car.GetComponent<Inventory>().CheckProtection();

        if ( canvas.GetComponent<Tarolo>().selected_name != "none")
        {
            
            car.GetComponent<Inventory>().UpdateProtection(canvas.GetComponent<Tarolo>().selected_stats, canvas.GetComponent<Tarolo>().selected_name);


            canvas.GetComponent<Tarolo>().selected_name = "none";
            canvas.GetComponent<Tarolo>().selected_stats = 0;
            shop.GetComponent<Shop>().HelpClose();
            shop2.GetComponent<CoronaShop>().chekhide();

        }


    }



}
