using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuggUtCheck : MonoBehaviour
{
    public GameObject kicsicucc;
    public GameObject harmas;

    // Start is called before the first frame update
    void Start()
    {
        if (!Physics2D.OverlapPoint(transform.position + new Vector3(3f, 0)))
        {
            Instantiate(kicsicucc, transform.position + new Vector3(1.4f + 2.1f, 0), Quaternion.identity, transform);
        }
        if (!Physics2D.OverlapPoint(transform.position + new Vector3(-3f, 0)))
        {
            Instantiate(kicsicucc, transform.position - new Vector3(1.4f + 2.1f, 0), Quaternion.identity, transform);
        }
    }
}
