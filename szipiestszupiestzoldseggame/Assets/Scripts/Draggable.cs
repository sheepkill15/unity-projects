using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject prefab;
    public int cost;
    
    public static bool Dragging;
    public static int PendingCost;

    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Overlay.Instance.Hide();
        if (Dragging) return;
        if (Player.coins < cost) return;
        Vector3 pos = Input.mousePosition;
        pos.z = 0;
        Instantiate(prefab, pos, Quaternion.identity);
        Dragging = true;
        PendingCost = cost;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Dragging) return;
        Overlay.Instance.ShowShopOverlay(_transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Dragging) return;
        Overlay.Instance.ShowShopOverlay(_transform);
    }
}
