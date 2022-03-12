using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickkkkkk : MonoBehaviour
{
    public GameObject stop;

    public void Click(GameObject game)
    {
        bool ja = false;
        if (!game.activeSelf)
            ja = true;
        GameObject.Find("Canvas").GetComponent<Tarolo>().DisableAllPanels();
        game.SetActive(ja);
        stop.GetComponent<CameraControl>().canmove = !ja;
    }
}
