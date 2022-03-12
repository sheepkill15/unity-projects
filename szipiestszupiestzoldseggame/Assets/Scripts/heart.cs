using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heart : MonoBehaviour
{
    public int health;
    public GameObject F;
    public Image healthbar;
    private int maxhealth;

    private void Start()
    {
        maxhealth = health;
    }

    public void ouch(int dam)
    {
        healthbar.fillAmount = health / (float) maxhealth;
        health -= dam;
        if (health <= 0)
        {
            FF();
            Destroy(gameObject);
        }

    }


    public void FF()
    {
        F.SetActive(true);
    }

}
