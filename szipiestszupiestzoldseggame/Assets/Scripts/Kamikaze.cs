using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : MonoBehaviour
{

    public int health;
    public float speed;
    public int strength;
    public int money;
    public Transform me;
    Vector2 p;
    public GameObject target;


    void Start()
    {
        me = transform;

    }


    void Update()
    {
            p = me.position;
            p -= new Vector2(speed, 0) * Time.deltaTime;
            me.position = p;

        RaycastHit2D hit = Physics2D.Raycast(me.position, Vector2.down, 5f,LayerMask.GetMask("Ally"));
        if (hit)
        {
            if (hit.transform.gameObject.tag == "Flower")
            {
              target = hit.transform.gameObject;
              dealdmg();
            }
            else if (hit.transform.gameObject.tag == "Heart")
            {
                target = hit.transform.gameObject;
                dealdmg2heart();
            }

        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "bullet")
        {
            int dam = collision.gameObject.GetComponent<bullet>().damage;
            tookdamage(dam);
        }

    }



    public void dealdmg()
    {

          
        target.gameObject.GetComponent<Flower>().ouch(strength);
        Spawn.hulla++;
        Destroy(this.gameObject);
   

    }
    public void dealdmg2heart()
    {

           
        target.gameObject.GetComponent<heart>().ouch(strength);
        Spawn.hulla++;
        Destroy(this.gameObject);

    }



    private void tookdamage(int amm)
    {
        health -= amm;
        if (health <= 0)
        {
            Spawn.hulla++;
            Player.coins += money;
            Destroy(this.gameObject);
        }



    }



}

