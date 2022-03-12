using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndExplode : MonoBehaviour
{
    private Transform _transform;

    public float speed = 4;

    public float lifetime = 10;
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        StartCoroutine(Die());
    }

    // Update is called once per frame
    void Update()
    {
        _transform.Translate(Vector3.forward * (speed * Time.deltaTime), Space.Self);
    }

    private void OnCollisionEnter(Collision other)
    {
        StopAllCoroutines();
        Debug.Log("Viszlat kegyetlen vilag");
        Destroy(gameObject);
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
