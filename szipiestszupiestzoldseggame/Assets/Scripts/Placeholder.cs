using System;
using UnityEngine;

public class Placeholder : MonoBehaviour
{
    private Transform _transform;

    private Camera _mainCamera;

    public int index;
    public bool akarhova;

    private bool _overlapping;

    private SpriteRenderer[] _renderers;

    private void Start()
    {
        _transform = transform;
        _mainCamera = Camera.main;
        _renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Vector3 pos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.y = Mathf.Clamp(pos.y, -3.04f, float.MaxValue);
        float holY = (int) (pos.y / GridManager.GridSize) * GridManager.GridSize - 0.5f;
        int holX = (int)(pos.x / GridManager.GridSize) * GridManager.GridSize;
    
        var overlap = Physics2D.OverlapPoint(_transform.position, LayerMask.GetMask("Ally"));
        if (overlap)
        {
            _overlapping = true;
            foreach (var renderer in _renderers)
            {
                renderer.color = Color.red;
            }
        }
        else if (_overlapping)
        {
            _overlapping = false;
            foreach (var renderer in _renderers)
            {
                renderer.color = Color.white;
            }
        }
        if (!_overlapping && Input.GetMouseButtonUp(0))
        {
            Player.coins -= Draggable.PendingCost;
            Draggable.Dragging = false;
            GameObject spawned = Instantiate(GridManager.Instance.spawnable[index], _transform.position, _transform.rotation,
                GridManager.Instance.map);
            Flower flower = spawned.GetComponent<Flower>();
            if (flower)
            {
                flower.index = index;
                GridManager.Instance.RegisterFlower(flower);
            }

            spawned.GetComponent<Tower>().cost = Draggable.PendingCost;
            
            Destroy(gameObject);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3 scale = _transform.eulerAngles;
            if (scale.y == 0) scale.y = 180;
            else if (Math.Abs(scale.y - 180) < 0.1f) scale.y = 0;
            _transform.eulerAngles = scale;
        } 
        if (!akarhova)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(holX, pos.y + 1f), Vector2.down, 100, LayerMask.GetMask("Map"));
            if (hit)
            {
                holY = hit.point.y + 1;
            }
        }
        
        _transform.position = new Vector3(
            holX, 
            holY, 
            0);
    }
}
