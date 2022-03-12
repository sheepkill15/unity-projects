using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int health;
    public float speed;
    public int strength;
    public float delay;
    public int money;
    public Transform me;
    Vector2 p;
    public GameObject target;
    public bool move = true;

    Coroutine deal;

    private bool _hit;

    public bool odavissa;
    public int irany = 1;

    private bool cooldown;

    void Awake()
    {
        me = transform;
    }

    public void Rotate()
    {
        irany = -irany;
        Vector3 scale = me.localScale;
        scale.x = -scale.x;
        me.localScale = scale;
    }
    
    void Update()
    {
        var hit = Physics2D.Raycast(me.position, me.right, 0.5f, LayerMask.GetMask("Ally"));
        if (hit)
        {
            move = false;
            if (!_hit)
            {
                if (hit.transform.gameObject.CompareTag("Flower"))
                {
                    target = hit.transform.gameObject;
                    deal = StartCoroutine(dealdmg());
                }
                else if (hit.transform.gameObject.CompareTag("Heart"))
                {
                    target = hit.transform.gameObject;
                    deal = StartCoroutine(dealdmg2heart());
                }

                _hit = true;
            }
        }
        else move = true;

        if (odavissa)
        {
            hit = Physics2D.Raycast(me.position, Vector2.down, 2f, LayerMask.GetMask("Map"));
            if (!cooldown && !hit)
            {

                Rotate();
                StartCoroutine(Cooldown());
            }
        }

        if (move || odavissa)
        {
            p = me.position;
            p -= new Vector2(speed * irany, 0) * Time.deltaTime;
            me.position = p;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            int dam = collision.gameObject.GetComponent<bullet>().damage;
            tookdamage(dam);
        }
    }
    //     else if (collision.gameObject.tag == "Flower")
    //     {
    //         target = collision.gameObject;
    //         deal = StartCoroutine(dealdmg());
    //     }
    //     else if (collision.gameObject.tag == "Heart")
    //     {
    //         target = collision.gameObject;
    //         deal = StartCoroutine(dealdmg2heart());
    //     }
    // }
    //     
    //         
    //
    //
    // }
    //
    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if(collision.gameObject.CompareTag("Flower") || collision.gameObject.CompareTag("Heart"))
    //         move = true;
    // }
    //
    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     if(other.gameObject.CompareTag("Flower") || other.gameObject.CompareTag("Heart"))
    //         move = false;
    // }
    private IEnumerator Cooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(0.5f);
        cooldown = false;
    }
    IEnumerator dealdmg()
    {
        while(true)
        {
            if (target.gameObject == null) break;
        target.gameObject.GetComponent<Flower>().ouch(strength);
        yield return new WaitForSeconds(delay);
        }

        _hit = false;


    }
    IEnumerator dealdmg2heart()
    {
        while (true)
        {
            if (target.gameObject == null) break;
            target.gameObject.GetComponent<heart>().ouch(strength);
            yield return new WaitForSeconds(delay);
        }

        _hit = false;

    }



    private void tookdamage( int amm )
    {
        health -= amm;
        if ( health <= 0 )
        {
            Spawn.hulla++;
            Player.coins += money;
            Destroy(gameObject);
        }



    }



}
