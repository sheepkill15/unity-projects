using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlikkCheck : MonoBehaviour
{
    private void Start()
    {
        SpawnMoreMap();
    }

    private void SpawnMoreMap()
    {
        if(Physics2D.OverlapPointAll(transform.position + new Vector3(2, 2)).Length == 0)
        {
            GameObject.Find("Map").GetComponent<Generate>().Epits(transform.position, 0);
        }
        if (Physics2D.OverlapPointAll(transform.position + new Vector3(-2, 2)).Length == 0)
        {
            GameObject.Find("Map").GetComponent<Generate>().Epits(transform.position, 2);
        }
        if (Physics2D.OverlapPointAll(transform.position + new Vector3(2, -2)).Length == 0)
        {
            GameObject.Find("Map").GetComponent<Generate>().Epits(transform.position, 1);
        }
        if (Physics2D.OverlapPointAll(transform.position + new Vector3(-2, -2)).Length == 0)
        {
            GameObject.Find("Map").GetComponent<Generate>().Epits(transform.position, 3);
        }
    }
}
