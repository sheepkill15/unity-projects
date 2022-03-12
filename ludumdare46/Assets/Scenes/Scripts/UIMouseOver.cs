using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image[] images;
    private Text[] texts;

    private void Start()
    {
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<Text>();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Image image in images)
        {
            Color color = image.color;
            color.a = 0.40f;
            image.color = color;
        }
        foreach (Text text in texts)
        {
            Color color = text.color;
            color.a = 0.40f;
            text.color = color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (Image image in images)
        {
            Color color = image.color;
            color.a = 1;
            image.color = color;
        }
        foreach (Text text in texts)
        {
            Color color = text.color;
            color.a = 1;
            text.color = color;
        }
    }
}
