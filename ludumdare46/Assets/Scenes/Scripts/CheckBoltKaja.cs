using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBoltKaja : MonoBehaviour
{
    Bolt bolt;
    public int protection;
    public int fertozes = 10;
    public GameObject rep;
    Tarolo tarolo;
    void Start()
    {
        bolt = GetComponentInParent<Bolt>();
        tarolo = GameObject.Find("Canvas").GetComponent<Tarolo>();
        rep = tarolo.rep;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Inventory>().fertozot == true)
        {
            StartCoroutine(meghal());
        }
        if (bolt.stock > 0)
        {
            fertozes = 8;
            int p;
            p = collision.GetComponent<Inventory>().tolt(bolt.stock);
            bolt.stock += -p;

            if (collision.GetComponent<Inventory>().fertozot == false)
            {
                protection = collision.GetComponent<Inventory>().protection_stats;

                fertozes = fertozes - Mathf.RoundToInt(protection / 100f * fertozes);
                //Debug.Log(fertozes);
                int random = Random.Range(1, 101);

                if (random <= fertozes)
                    collision.GetComponent<Inventory>().Fertozes();
            }
        }
        if(bolt.stock == 0)
        {
            bolt.ownRenderer.color = new Color(1, 1, 1, 0.25f);
        }
    }

    IEnumerator meghal()
    {
        yield return new WaitForSeconds(Random.Range(15, 30));
        Debug.Log("asd");
        if (bolt.workers == 1)
        {
            bolt.workers--;
            rep.GetComponent<Reputation>().change(-10);
        }
        else if (bolt.workers > 1)
        {
            bolt.workers--;
            rep.GetComponent<Reputation>().change(-2);
        }
        tarolo.workers_alive--;
    }

}
