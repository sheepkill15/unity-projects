using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Population_Counter : MonoBehaviour
{

    public int alive;
    public Text alive_txt;
    public int dead;
    public Text dead_txt;

    public void UpdateUI( int newdeath , int newalive )
    {
        alive += newalive;
        dead += newdeath;

        dead_txt.text = dead.ToString();
        alive_txt.text = alive.ToString();
    }


}
