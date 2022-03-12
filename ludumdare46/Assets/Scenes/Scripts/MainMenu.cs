using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(BoxCollider2D))]
public class MainMenu : MonoBehaviour
{
    private SpriteRenderer ownRenderer;

    public int index;
    public GameObject help;

    // Start is called before the first frame update
    void Start()
    {
        ownRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        Color color = ownRenderer.color;
        color.a = 1;
        ownRenderer.color = color;
    }

    private void OnMouseExit()
    {
        Color color = ownRenderer.color;
        color.a = 0.40f;
        ownRenderer.color = color;
    }

    private void OnMouseDown()
    {
        switch(index)
        {
            case 0:
                SceneManager.LoadScene("SampleScene");
                break;
            case 1:
                help.SetActive(!help.activeSelf);



                
                break;
            case 2:
                Application.Quit();
                break;
        }
    }
}
