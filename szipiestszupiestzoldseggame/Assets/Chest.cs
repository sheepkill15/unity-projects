using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    private bool gave;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (gave) return;
        if (other.gameObject.name == "Karakter")
        {
            Player.coins += Random.Range(10, 15) * Spawn.maxwave;
            gave = true;
        }
    }
}
