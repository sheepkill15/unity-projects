using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public AudioSource source;
    public CameraControl thiCamera;

    public GameObject pausemenu;
    bool paused = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pause();
        }
    }

    public void Pause()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0;
            source.Pause();
            thiCamera.canmove = false;
          //  pausemenu.SetActive(true);
        }

        else
        {
            Time.timeScale = 1;
            source.UnPause();
            thiCamera.canmove = true;
         //   pausemenu.SetActive(false);
        }
    }
}
