using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    public static Overlay Instance;
    private Camera _mainCamera;

    public RectTransform towerOverlay;
    private Transform _tower;

    public RectTransform shopOverlay;
    private Transform _shop;
    
    // Start is called before the first frame update
    private void Start()
    {
        _mainCamera = Camera.main;
        Instance = this;
    }

    private void Update()
    {
        if (towerOverlay.gameObject.activeSelf)
        {
            towerOverlay.position = _mainCamera.WorldToScreenPoint(_tower.position + new Vector3(0, 2f, 0));
        }

        if (shopOverlay.gameObject.activeSelf)
        {
            shopOverlay.position = _shop.position + new Vector3(0, 100, 0);
        }
    }

    public void ShowTowerOverlay(Transform tower)
    {
        if (Draggable.Dragging) return;
        if (towerOverlay.gameObject.activeSelf)
        {
            Hide();
            return;
        }
        towerOverlay.gameObject.SetActive(true);
        _tower = tower;
    }
    
    public void ShowShopOverlay(Transform shop)
    {
        if (Draggable.Dragging) return;
        if (shopOverlay.gameObject.activeSelf)
        {
            Hide();
            return;
        }
        shopOverlay.gameObject.SetActive(true);
        shopOverlay.GetComponentInChildren<Text>().text = $"Cost: {shop.GetComponent<Draggable>().cost}";
        _shop = shop;
    }
    

    public void Hide()
    {
        towerOverlay.gameObject.SetActive(false);
        shopOverlay.gameObject.SetActive(false);
    }

    public void RemoveClicked()
    {
        Hide();
        Player.coins += _tower.GetComponent<Tower>().cost / 2;
        Destroy(_tower.gameObject);
    }
}
