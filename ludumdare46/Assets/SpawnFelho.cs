using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFelho : MonoBehaviour
{
    public int esely = 60;
    public GameObject hatar;
    public GameObject felho;
    int kezd = 80;
    int veg = 260;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawn());
    }

    private void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.F))
        {
            spawnfelho();
        }*/
    }

    IEnumerator spawn()
    {
        yield return new WaitForSeconds(Random.Range(kezd, veg));
        spawnfelho();
    }

    void spawnfelho()
    {
        Vector3 hatarpos = hatar.transform.position;
        hatarpos.y += Random.Range(-50, 50);
        hatar.transform.position = hatarpos;

        Instantiate(felho, -hatar.transform.position, Quaternion.identity, transform).GetComponent<Felho>().hatar = hatar;
        kezd = kezd / 2;
        veg = veg / 2;
        spawn();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<FertozesTracker>() != null)
        {
            collision.GetComponent<FertozesTracker>().Fertoz(esely);
        }
    }
}
