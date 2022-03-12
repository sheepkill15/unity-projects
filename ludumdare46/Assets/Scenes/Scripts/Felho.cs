using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Felho : MonoBehaviour
{
    Seeker seeker;
    AIPath ai;

    public GameObject hatar;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        ai = GetComponent<AIPath>();
        seeker.StartPath(transform.position, hatar.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        if (!ai.hasPath)
            Destroy(gameObject);
    }
}
