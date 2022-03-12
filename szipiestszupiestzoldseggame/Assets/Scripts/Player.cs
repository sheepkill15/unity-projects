using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static int cango = 0;

    public static int health = 3;
    public float recoveryTime = 1f;
    private bool _recover;

    public Text text;



    public GameObject F;

    private SpriteRenderer[] _renderer;

    public static int coins=500;

    private void Start()
    {
        _renderer = GetComponentsInChildren<SpriteRenderer>();
    }

    private IEnumerator Cooldown()
    {
        Color color = Color.white;
        foreach (var rend in _renderer)
        {
            color = rend.color;
            color.a = 0.5f;
            rend.color = color;
        }
        
        yield return new WaitForSeconds(recoveryTime);
        foreach (var rend in _renderer)
        {
            color.a = 1f;
            rend.color = color;
            _recover = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_recover) return;
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("oof");
            health--;
            text.text = health.ToString();
            if (health < 0)
            {
                F.SetActive(true);
            }

            _recover = true;
            StartCoroutine(Cooldown());
        }
    }

    private void GOTOOTHERWORLD(string scene)
    {
        if ( cango < 2)
        {
             SceneManager.LoadScene(scene);
            cango ++;
        }
    }

    private void PickupCoin(GameObject coin)
    {
        int add = coin.GetComponent<Coin>().worth;
        Destroy(coin);
        coins+=add;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Portal"))
        {
            GOTOOTHERWORLD(other.gameObject.name);
        }
        else if (other.gameObject.CompareTag("Coin"))
        {
            PickupCoin(other.gameObject);
        }
    }

    public void takedmg()
    {
        health--;
        text.text = health.ToString();
        if (health < 0)
        {
            F.SetActive(true);
        }
    }


}
