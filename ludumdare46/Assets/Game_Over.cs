using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Over : MonoBehaviour
{

    public GameObject People;
    public int Dead;
    public int Alive;
    public int MaxAlive = 0;
    public float reputation;
    public GameObject rep;

    public Tarolo tarolo;


    public Text reason;
    public GameObject gameover;



    void Update()
    {
        Dead = People.GetComponent<Population_Counter>().dead;
        Alive = People.GetComponent<Population_Counter>().alive;
        reputation = rep.GetComponent<Reputation>().rep;

        if(tarolo.workers_alive == 0)
        {
            GameOver();
        }


        if (MaxAlive < Alive)
            MaxAlive = Alive;

        if (reputation == 0)
            GameOver();


        if (Alive == 0)
            GameOver();

     //   if ( Input.GetKeyDown("f"))
      //  {
          //  tarolo.workers_alive = 0;
      //  }

    }
    void GameOver()
    {
        Debug.Log("GameOver");
        gameover.SetActive(true);
        if(reputation == 0)
        {
            reason.text = "You have reached a bad reputation!";
        }
        else if(Alive == 0)
        {
            reason.text = "Too many people have died!";
        }
        else if (tarolo.workers_alive == 0)
        {
            reason.text = "Too many workers have died!";
        }
    }

    public void restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
