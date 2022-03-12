using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextpage : MonoBehaviour
{
    public GameObject text1;
    public GameObject text2;
    public GameObject button1;
    public GameObject button2;


    public void Text1()
    {
        text1.SetActive(false);
        text2.SetActive(true);
        button1.SetActive(false);
        button2.SetActive(true);
    }

    public void Text2()
    {
        text2.SetActive(false);
        text1.SetActive(true);
        button2.SetActive(false);
        button1.SetActive(true);
    }
}
