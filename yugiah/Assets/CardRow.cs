using System.Collections.Generic;
using UnityEngine;

public class CardRow : MonoBehaviour
{
    public int slotsNumber;
    public GameObject placeholderPrefab;

    private RectTransform _transform;

    public List<CardSlot> slots;
    
    // Start is called before the first frame update
    private void Start()
    {
        _transform = GetComponent<RectTransform>();
        CreateOrResizeSlots();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void CreateOrResizeSlots()
    {
        for (int i = 0; i < slotsNumber; i++)
        {
            slots.Add(Instantiate(placeholderPrefab, _transform).GetComponent<CardSlot>());
        }
    }
}
