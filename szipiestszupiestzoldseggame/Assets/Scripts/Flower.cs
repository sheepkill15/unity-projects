using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{

    public int health;
    public GameObject golyo;
    public float delay;
    public int index;
    public bool shouldlo;

    public new Transform transform;


    void Awake()
    {
        transform = gameObject.transform;

    }

    private void Start()
    {
        
        StartCoroutine(pew());
    }



    public void ouch( int dam)
    {
        health -= dam;
        if (health <= 0)
            Destroy(gameObject);
    }






    IEnumerator pew()
    {
        while(shouldlo)
        {
            GameObject cucc = Instantiate(golyo, transform.position, transform.rotation, transform);
            yield return new WaitForSeconds(delay);
        }

    }



}
