using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    public Text coinsText;

    // Update is called once per frame
    void Update()
    {
        coinsText.text = Player.coins.ToString();
    }
}
