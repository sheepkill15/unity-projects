using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGombok : MonoBehaviour
{
    public void creatorGomb()
    {
        // character creator
        SceneManager.LoadScene("Character_creator");
    }

    public void studioGomb()
    {
        // studio
    }

    public void playGomb()
    {
        // play rpg
    }

    public void settingsGomb()
    {
        // settings
    }
}
