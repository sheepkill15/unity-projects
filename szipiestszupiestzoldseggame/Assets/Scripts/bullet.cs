using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    public int damage;
    public float speed;
    public int range;
    public Transform me;
    Vector2 p;
    float startpos;

    public int direction = 1;


    void Start()
    {
        me = transform;
        startpos = me.transform.position.x;
    }

    void Update()
    {
        p = me.position;
        p += (Vector2)(direction == 1 ? me.right : -me.up) * (speed * Time.deltaTime);
        me.position = p;

        if ( transform.position.x - startpos > range || startpos - transform.position.x > range )
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
