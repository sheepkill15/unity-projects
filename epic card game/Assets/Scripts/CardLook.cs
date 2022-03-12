using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardLook : MonoBehaviour
{
    public Card card;
    
    public TMP_Text nameText;

    public TMP_Text descriptionText;

    public Image iconImage;

    public TMP_Text costText;
    public TMP_Text damageText;
    public TMP_Text healthText;

    // Start is called before the first frame update
    private void Start()
    {
        if(nameText is { })
            nameText.text = card.info.cardName;
        if(descriptionText is { })
            descriptionText.text = card.info.description;
        if(iconImage is { })
            iconImage.sprite = card.info.icon;
        if(costText is { })
            costText.text = card.stats.cost.ToString();
        if (card.info.type != CardInfo.Type.Spell) return;
        damageText.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (card.info.type == CardInfo.Type.Spell) return;
        damageText.text = card.stats.damage.ToString();
        healthText.text = card.stats.health.ToString();
    }
}
