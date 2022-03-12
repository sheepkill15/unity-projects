using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public struct Item
    {
        public int Index;
        public Vector2 Position;
        public Quaternion Rotation;
    }
    
    public static int GridSize { get; } = 1;
    public static GridManager Instance;

    public static List<Item> items;

    public GameObject[] spawnable;
    public Transform map;
    
    private void Start()
    {
        Instance = this;
        if (items != null)
        {
            foreach (Item item in items)
            {
                Instantiate(spawnable[item.Index], item.Position, item.Rotation,
                    map);
            }
                
        }
    }

    public void RegisterFlower(Flower flower)
    {
        if (items == null)
        {
            items = new List<Item>();
        }
        Item flItem = new Item
        {
            Index = flower.index, Position = flower.transform.position, Rotation = flower.transform.rotation
        };
        items.Add(flItem);
    }
}
