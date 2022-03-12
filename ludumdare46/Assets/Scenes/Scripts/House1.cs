using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class House1 : MonoBehaviour
{
    public int residents;
    public int deaths=0;
    public Text residents_txt;
    public Text deaths_txt;
    public GameObject HouseCanvas;
    Tarolo tarolo;
    public GameObject panel;
    public int Active = 0;
    public Vector2 housepos;
    public GameObject PeopleCounter;
    public GameObject bolt;
    GameObject dialogBox;
    bool kapotte = false;
    public bool keri = false;
    int random;
    public GameObject rep;

    public GameObject nagyx;

    private GameObject DialogBox { get
        {
            if(dialogBox == null)
            {
                dialogBox = Instantiate(tarolo.dialog, transform.position + new Vector3(3.5f, 3.5f), Quaternion.identity, transform);
            }
            return dialogBox;
        }
        set => dialogBox = value; }

    void Start()
    {
        HouseCanvas = GameObject.Find("Canvas");
        tarolo = HouseCanvas.GetComponent<Tarolo>();
        panel = HouseCanvas.GetComponent<Tarolo>().HousePanel;
        residents_txt = tarolo.residenttxt;
        deaths_txt = tarolo.deathstxt;
        residents = Random.Range(1, 6);
        housepos = transform.position;
        bolt = tarolo.ShopPanel;
        rep = tarolo.rep;


        PeopleCounter = GameObject.Find("People");
        PeopleCounter.GetComponent<Population_Counter>().UpdateUI(0,residents);
        random = Random.Range(1, 101);

        if (random < 11)
            StartCoroutine(VarjukAKajat(0, 120));
        else
            StartCoroutine(VarjukAKajat(120, 1000)); //eleje
    }


    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        {
            panel.SetActive(true);
            bolt.SetActive(false);
        }

        residents_txt.text = residents.ToString();
        deaths_txt.text = deaths.ToString();
    }


    public void Die()
    {
        if ( residents > 0 )
        {
            deaths++;
            residents--;
            PeopleCounter.GetComponent<Population_Counter>().UpdateUI(1, -1);

        }
        if(residents == 0)
        {
            StopAllCoroutines();
            keri = false;
            DialogBox.SetActive(false);

            Instantiate(nagyx, transform.position, Quaternion.identity, transform);
        }
        if ( Active == 1 )
        {
            residents_txt.text = residents.ToString();
            deaths_txt.text = deaths.ToString();
        }

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //hmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmsssssssssssss
            if (mousepos.x - housepos.x < 3.5f && mousepos.x - housepos.x > -3.5f && mousepos.y - housepos.y < 3.5f && mousepos.y - housepos.y > -3.5f)//hmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm
            {
                Active = 1; 
            }
            else
                Active = 0;
        }

       if (Input.GetKeyDown(KeyCode.F))
       {
           Die();
       } 
       
    }

    IEnumerator VarjukAKajat(int kezd, int veg)
    {
        yield return new WaitForSeconds(Random.Range(kezd, veg)); //anyit var a rendelesig
        keri = true;
        kapotte = false;
        Text text;
        DialogBox.SetActive(true);
        text = DialogBox.GetComponentInChildren<Text>();
        int i = Random.Range(60, 150);    //menyit var
        string percek = "";
        if ((i % 60) / 10 == 0) percek += "0";
        percek += (i % 60).ToString();
        text.text = (i / 60).ToString() + ":" + percek;
        while(i > 0)
        {
            yield return new WaitForSeconds(1);
            i--;
            percek = "";
            if ((i % 60) / 10 == 0) percek += "0";
            text.text = (i / 60).ToString() + ":" + percek + (i % 60).ToString();
        }
        DialogBox.SetActive(false);
        if(!kapotte)
        {
            Die();
            StartCoroutine(VarjukAKajat(60, 120)); //nem kapot kajat ujra
        }
        else
        {
            Etet();
            keri = false;
        }
    }

    public void Etet()
    {
        rep.GetComponent<Reputation>().change(0.2f);
        keri = false;
        kapotte = true;
        StopAllCoroutines();
        DialogBox.SetActive(false);
        StartCoroutine(VarjukAKajat(120, 500));    //kapot kajat ujra
    }

    public void NemEtet()
    {
        if(dialogBox != null)
            DialogBox.GetComponentInChildren<Text>().text = "Bring food";
    }

}
