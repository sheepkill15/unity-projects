using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertozodj : MonoBehaviour
{
    public int esely = 25;
    Seeker seeker;
    AIPath ai;


    private void Start()
    {
        seeker = GetComponent<Seeker>();
        StartCoroutine(tunjel());
        ai = GetComponent<AIPath>();
    }

    private void Update()
    {
        if(!ai.hasPath)
            seeker.StartPath(transform.position, Random.insideUnitCircle * 50);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<FertozesTracker>() != null)
            collision.GetComponent<FertozesTracker>().Fertoz(esely);
    }

    IEnumerator tunjel()
    {
        yield return new WaitForSeconds(Random.Range(60,120));
        Destroy(gameObject);
    }


}
