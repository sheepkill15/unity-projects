using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bolt : MonoBehaviour
{
    public GameObject ShopCanvas;
    public GameObject HouseCanvas;
    public GameObject panel;
    public int workers;
    public Text workers_txt;
    public int stock;
    public Text stock_txt;
    Tarolo tarolo;

    public SpriteRenderer ownRenderer;

    public GameObject nagyx;
    void Start()
    {
        ShopCanvas = GameObject.Find("Canvas");
        tarolo = ShopCanvas.GetComponent<Tarolo>();
        HouseCanvas = ShopCanvas.GetComponent<Tarolo>().HousePanel;
        panel = ShopCanvas.GetComponent<Tarolo>().ShopPanel;
        stock_txt = tarolo.stocktxt;
        workers_txt = tarolo.workerstxt;

        workers = Random.Range(1, 6);
        stock = Random.Range(1, 6);
        stock = stock * workers;

        if(ShopCanvas.GetComponent<Tarolo>().workers_alive != -1)
            ShopCanvas.GetComponent<Tarolo>().workers_alive += workers;
        else ShopCanvas.GetComponent<Tarolo>().workers_alive = workers;

        ownRenderer = GetComponent<SpriteRenderer>();

        workers_txt.text = workers.ToString();
        stock_txt.text = stock.ToString();
        if ( workers > 0 )
            StartCoroutine(ReStock());
    }


    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        panel.SetActive(true);
        HouseCanvas.SetActive(false);
        workers_txt.text = workers.ToString();
        stock_txt.text = stock.ToString();
    }



    IEnumerator ReStock()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(120, 220));
            if (workers > 0)
            {
                stock = Mathf.Min(workers * 6, stock + Random.Range(1, 6) * workers);
                ownRenderer.color = new Color(1, 1, 1, 1);
            }
            else break;
        }
    }


    public void Update()
    {
        if (workers < 1)
        {
            stock = 0;

            Instantiate(nagyx, transform.position, Quaternion.identity, transform);
            Debug.Log("oyy wtf");
        }
    }

}

