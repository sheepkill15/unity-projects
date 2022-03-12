using UnityEngine;
using UnityEngine.UI;

public class Char_gombok : MonoBehaviour
{
    public GameObject[] testreszeink;
    public GameObject[] fej_reszek_tobb;

    public GameObject fo_menu_szupercucc_mac;
    public GameObject lista;

    private int mostHolVagyunk = 0;

    public void kivalasztva(Dropdown mcucc)
    {
        testreszeink[mostHolVagyunk].SetActive(false);
        switch(mcucc.value)
        {
            case 0: // fej
                testreszeink[0].SetActive(true);
                mostHolVagyunk = 0;
                break;
            case 1: // test
                testreszeink[1].SetActive(true);
                mostHolVagyunk = 1;
                break;
            case 2: // kez
                testreszeink[2].SetActive(true);
                mostHolVagyunk = 2;
                break;
            case 3: // lab
                testreszeink[3].SetActive(true);
                mostHolVagyunk = 3;
                break;

            default:
                Debug.Log("valamit nagyon elcsesztel");
                break;
        }
    }

    public void klikkFej(int a)
    {
        testreszeink[a].SetActive(false);
        fej_reszek_tobb[a].SetActive(true);
    }

    public void kicsiBackGomb(int a)
    {
        fej_reszek_tobb[a].SetActive(false);
        if (a == 0)
        {
            testreszeink[a].SetActive(true);
        }
    }

    public void nagyBackGomb()
    {
        gameObject.SetActive(false);
        fo_menu_szupercucc_mac.SetActive(true);
    }

    public void osszesItem(int a)
    {
        fej_reszek_tobb[a].SetActive(false);
        lista.SetActive(true);
    }
}
