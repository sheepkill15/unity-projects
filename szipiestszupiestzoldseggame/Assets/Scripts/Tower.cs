using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private Transform _transform;
    public int cost;
    
    // Start is called before the first frame update
    private void Start()
    {
        _transform = transform;
    }

    private void OnMouseDown()
    {
        Overlay.Instance.ShowTowerOverlay(_transform);
    }
}
