using UnityEngine;
using UnityEngine.UI;
public class GombListaGomb : MonoBehaviour
{
    public void SetImage(Sprite image)
    {
        GetComponent<Image>().sprite = image;
    }

    public void OnClick()
    {

    }
}
