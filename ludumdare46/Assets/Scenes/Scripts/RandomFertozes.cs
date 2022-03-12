using System.Collections;
using UnityEngine;

public class RandomFertozes : MonoBehaviour
{
    int esely = -10;
    public GameObject fertozes;

    // Start is called before the first frame update
    void Start()
    {
        esely = Random.Range(-20, 0);
        StartCoroutine(spawn());
        StartCoroutine(novekedes());
    }

    IEnumerator spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(30);
            if (Random.Range(0, 100) < esely)
            {
                GameObject made = Instantiate(fertozes, transform.position, Quaternion.identity, transform);
                /*    if(transform.rotation.eulerAngles.z > 91 && transform.rotation.eulerAngles.z < 89)
                        made.transform.position += new Vector3(Random.Range(-3.5f, 3.5f),0);
                    else made.transform.position += new Vector3(0, Random.Range(-3.5f, 3.5f));*/
                esely = Random.Range(-20, 0);
            }
        }
    }

    IEnumerator novekedes()
    {
        while(true)
        {
            yield return new WaitForSeconds(30);
            esely++;
        }
    }
}
