using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoviraggolyo : MonoBehaviour
{

    public int damage;
    public float speed;
    public int range;
    public Transform me;
    Vector2 p;
    float startpos;


    void Start()
    {
        me = transform;
        startpos = me.transform.position.y;
    }

    void Update()
    {
        p = me.position;
        p += (Vector2)(-me.up) * (speed * Time.deltaTime);
        me.position = p;

        if (transform.position.y - startpos > range || startpos - transform.position.y > range)
        {
            Destroy(this.gameObject);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
