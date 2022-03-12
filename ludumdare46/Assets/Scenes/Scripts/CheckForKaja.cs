using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForKaja : MonoBehaviour
{
    House1 house;
    public GameObject bank;
    public int fertozes;
    public int protection;
    public GameObject rep;

    // Start is called before the first frame update
    void Start()
    {
        house = GetComponentInParent<House1>();
        bank = GameObject.Find("Canvas").GetComponent<Tarolo>().money;
        rep = GameObject.Find("Canvas").GetComponent<Tarolo>().rep;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (house.keri)
        {
            if (collision.GetComponent<Inventory>().fertozot == true)
                StartCoroutine(meghal());
            if (collision.GetComponent<Inventory>().kajadb > 0)
            {
                collision.GetComponent<Inventory>().urit();

                if ( collision.GetComponent<Inventory>().fertozot!=true)
                {
                    fertozes = 8;
                    protection = collision.GetComponent<Inventory>().protection_stats;
                    fertozes = fertozes - Mathf.RoundToInt(protection / 100f * fertozes);
                    int random = Random.Range(1, 101);

                    if (random <= fertozes)
                    {
                        collision.GetComponent<Inventory>().Fertozes();
                    }
                }
                house.Etet();
                int pay = (int)(Random.Range(80, 121) * rep.GetComponent<Reputation>().rep/100);
                bank.GetComponent<Money>().STONKS(pay);
            }
            else
            {
                house.NemEtet();
            }
        }
    }
    IEnumerator meghal()
    {
        Debug.Log("1");
        yield return new WaitForSeconds(Random.Range(15, 30));
        Debug.Log("2");
        house.GetComponent<House1>().Die();
        rep.GetComponent<Reputation>().change(-5);
    }
}
