using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertozesTracker : MonoBehaviour
{
    KocsiMozogj kocsi;
    Inventory inventory;

    public Inventory Inventory { get {
            if(inventory == null)
            {
                inventory = GetComponent<Inventory>();
            }
            return inventory;
        } set => inventory = value; }

    public void Fertoz(int esely)
    {
        int ujesely = esely - Mathf.RoundToInt(Inventory.protection_stats / 100f * esely);
        int kocka = Random.Range(0, 100);
        if (kocka <= ujesely)
        {
            Inventory.Fertozes();
        }
    }
}
