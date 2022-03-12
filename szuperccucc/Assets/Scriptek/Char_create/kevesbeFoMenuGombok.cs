using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kevesbeFoMenuGombok : MonoBehaviour
{
    public GameObject oltoztetos;

    public void oltoztetosMegnyit()
    {
        gameObject.SetActive(false);
        oltoztetos.SetActive(true);
    }
}
