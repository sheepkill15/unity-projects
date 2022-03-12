using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAdventure : MonoBehaviour
{
    public Transform me;
    Vector2 p;
    public float speed = 3;

    private int irany = 1;
    private bool cooldown;
    
    // Start is called before the first frame update
    void Start()
    {
        me = transform;
        Vector3 scale = me.localScale;
        scale.x = -scale.x;
        me.localScale = scale;
    }

    void Update()
    {
        var hit = Physics2D.Raycast(me.position, Vector2.down, 2f, LayerMask.GetMask("Map"));
        if (!cooldown && !hit)
        {
            
            irany = -irany;
            Vector3 scale = me.localScale;
            scale.x = -scale.x;
            me.localScale = scale;
            StartCoroutine(Cooldown());
        }
        p = me.position;
        p -= new Vector2(speed * irany, 0) * Time.deltaTime;
        me.position = p;
    }

    private IEnumerator Cooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(0.5f);
        cooldown = false;
    }
}
