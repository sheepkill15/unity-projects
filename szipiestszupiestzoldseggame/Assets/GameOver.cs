using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Restart()
    {
        GridManager.items = null;
        Player.health = 3;
        Player.cango = 0;
        Player.coins = 500;
        Spawn.hulla = 0;
        Spawn.maxwave = 2;
        Spawn.maxnum = 12;
        Spawn.maxt = 20;
        Spawn.mint = 10;
        Spawn.number = 0;
        Spawn.number2 = 0;
        Spawn.minnum = 7;
        Spawn.dinyechance = 5;
        Spawn.wave = 0;
        SceneManager.LoadScene("SampleScene");

    }

    public void Exit()
    {
        Application.Quit();
    }
}
