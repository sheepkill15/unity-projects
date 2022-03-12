using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int kajadb = 0;
    public int max=2;
    private int random;


    private GameObject inv1;
    private GameObject inv2;
    private GameObject inv3;
    private GameObject inv4;
    private GameObject inv5;
    private GameObject inv6;
    private GameObject fertozo_kep;
    private GameObject talal;
    private KocsiMozogj Kocsi;
    public GameObject rep;

    public Image protection_txt;

    public int protection_stats = 0;
    public Sprite protection_name;

    public bool highLighted = false;
    public bool fertozot = false;
    public bool tudjuk = false;


    public string kocsiname;


    public void Start()
    {
        protection_txt = GameObject.Find("Canvas").GetComponent<Tarolo>().protection_txt;
        inv3 = GameObject.Find("Canvas").GetComponent<Tarolo>().Inv3;
        inv1 = GameObject.Find("Canvas").GetComponent<Tarolo>().Inv1;
        inv2 = GameObject.Find("Canvas").GetComponent<Tarolo>().Inv2;
        inv4 = GameObject.Find("Canvas").GetComponent<Tarolo>().Inv4;
        inv5 = GameObject.Find("Canvas").GetComponent<Tarolo>().Inv5;
        inv6 = GameObject.Find("Canvas").GetComponent<Tarolo>().Inv6;
        fertozo_kep = GameObject.Find("Canvas").GetComponent<Tarolo>().infection;
        Kocsi = GetComponent<KocsiMozogj>();
        rep = GameObject.Find("Canvas").GetComponent<Tarolo>().rep;
    }

 /*   public void Update()
    {
        if (Input.GetKeyDown("f"))
            Fertozes();
    }*/

    public GameObject Inv1 { get
        {
            if(inv1 == null)
            {
                inv1 = Talal.GetComponent<Tarolo>().Inv1;
            }
            return inv1;
        }
        set => inv1 = value; }

    public GameObject Talal { get { 
            if(talal == null)
            {
                talal = GameObject.Find("Canvas");
            }
            return talal;
        } set => talal = value; }

    public GameObject Inv2 { get {
            if(inv2 == null)
            {
                inv2 = Talal.GetComponent<Tarolo>().Inv2;
            }
            return inv2;
        } set => inv2 = value; }

    public int tolt ( int raktar )
    {
        int p = 0;
        if ( kajadb < max )
        {
            while ( raktar > 0 && kajadb < max )
            {
                kajadb++;
                raktar--;
                p++;
            }
        }
        UpdateUI();
        return (p);
    }

    public void urit()
    {
        kajadb--;
        if(highLighted)
            UpdateUI();
    }

    public void Fertozes()
    {
        Debug.Log("Oh Shit");
        fertozot = true;
        StartCoroutine(HALAL());
    }



    IEnumerator HALAL()
    {
        yield return new WaitForSeconds(120);
        if (fertozot)
        {
            Kocsi.movecar.cars.Remove(Kocsi);
            Destroy(gameObject);
            Kocsi.movecar.DisplayCars();
            rep.GetComponent<Reputation>().change(-20);
        }
        StopAllCoroutines();
    }




    public void UpdateUI ()
    {

        if (tudjuk == true)
            fertozo_kep.SetActive(true);
        else
            fertozo_kep.SetActive(false);

        if (kajadb >= 1)
            inv1.SetActive(true);
        else
            inv1.SetActive(false);


        if (kajadb >= 2)
            inv2.SetActive(true);
        else
            inv2.SetActive(false);

        if (kajadb >= 3)
            inv3.SetActive(true);
        else
            inv3.SetActive(false);

        if (kajadb >= 4)
            inv4.SetActive(true);
        else
            inv4.SetActive(false);

        if (kajadb >= 5)
            inv5.SetActive(true);
        else
            inv5.SetActive(false);

        if (kajadb >= 6)
            inv6.SetActive(true);
        else
            inv6.SetActive(false);

        protection_txt.sprite = protection_name;

    }

    public void UpdateProtection( int stat , string name)
    {
        if ( name == "Check" )
        {
            if ( fertozot == true )
            {
                UpdateUI();
                tudjuk = true;
            }
        }
        else if ( name == "Med")
        {
            if ( tudjuk == true )
            {
                random = Random.Range(1, 101);

                if ( random <= stat)
                {

                    tudjuk = false;
                    fertozot = false;
                    UpdateUI();
                }
          

            }
        }
        else
        {
            protection_name = GameObject.Find("Canvas").GetComponent<Tarolo>().selected_image;
            protection_stats = stat;
        }
        UpdateUI();
    }

    public void checkall()
    {
        if (fertozot == true)
        {
            UpdateUI();
            tudjuk = true;
        }
    }

    public void CheckProtection ()
    {
       //Debug.Log(protection_stats);
    }

}
