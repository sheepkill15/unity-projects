using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    public GameObject[] platforms;
    public GameObject[] coins;
    public GameObject[] enemies;

    public GameObject portal;

    public float coinDropChance = 0.3f;
    public float enemySpawnChance = 0.3f;

    private Transform _transform;
    
    // Start is called before the first frame update
    private void Start()
    {
        _transform = transform;
        int mennyi = Random.Range(25, 50);
        float elozoMagassag = -2f;
        float elozoSzel = 0;
        float elozoPlatf = 0;
        int elojel = 1;
        if (Random.value > 0.5f)
        {
            elojel = -1;
        }
        for (int i = 0; i < mennyi; i++)
        {
           
            float magassag = Random.Range(0, 2f);
            GameObject platform = platforms[Random.Range(0, platforms.Length)];

            float szel = platform.GetComponent<SpriteRenderer>().size.x / 2f;
            
            elozoSzel += elojel * (szel + Random.Range(3, 6) + elozoPlatf);

            elozoPlatf = szel;

            elozoMagassag += magassag;
            Instantiate(platform, new Vector3(elozoSzel, elozoMagassag - 0.5f, 0), Quaternion.identity, _transform);

            if (i == mennyi - 1)
            {
                //TODO: SPAWN TREASURE CHEST
                Instantiate(portal, new Vector3(elozoSzel + elojel * Random.value * szel, elozoMagassag + 0.25f, 0),
                    Quaternion.identity, _transform).name = "SampleScene";
                return;
            }
            if (Random.value < coinDropChance)
            {
                GameObject coin = coins[0];
                float rand = Random.value;
                foreach (var availableCoin in coins)
                {
                    if (rand <= availableCoin.GetComponent<Coin>().rarity)
                    {
                        coin = availableCoin;
                    }
                }
                Instantiate(coin, new Vector3(elozoSzel + elojel * Random.value * szel, elozoMagassag + 1.25f, 0),
                    Quaternion.identity, _transform);
            }
            else if (Random.value < enemySpawnChance)
            {
                GameObject enemy = enemies[Random.Range(0, enemies.Length)];
                enemy = Instantiate(enemy, new Vector3(elozoSzel + elojel * Random.value * szel, elozoMagassag + 1.25f, 0),
                    Quaternion.identity, _transform);
                var enemyScr = enemy.GetComponent<Enemy>();
                enemyScr.odavissa = true;
                enemyScr.irany = -1;
                //enemyScr.Rotate();
            }

            
        }
    }
}
