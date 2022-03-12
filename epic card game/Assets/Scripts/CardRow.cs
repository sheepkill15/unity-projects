using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardRow : MonoBehaviour
{
    public enum RowSide
    {
        Friendly,
        Enemy,
        FriendlyHand,
        EnemyHand,
    }
    
    [Tooltip("Number of card slots in this row")]
    public int numCardSlots = 5;
    [Tooltip("Gap between card slots")]
    public float cardSlotGap = .3f;
    [Tooltip("The prefab to use for card slots")]
    public GameObject slotPrefab;
    [Tooltip("The transform from which the cards spawn")]
    public Transform startPoint;

    public RowSide rowSide;
    public bool hasPlaceholder = true;
    public bool allCanAttackAtStart;
    
    // TODO: store reference to card slots
    public List<BasicCard> cardSlots;
    private float _cardSlotWidth = 1;

    private Transform _rowTransform;
    

    private void Awake()
    {
        GameManager.RegisterCardRow(this);
        _rowTransform = transform;
    }

    // Start is called before the first frame update
    private void Start()
    {
        cardSlots ??= new List<BasicCard>(numCardSlots);
        _cardSlotWidth = slotPrefab.transform.localScale.x;
        startPoint ??= _rowTransform;

        UpdateSlotPositions();
        
        if(hasPlaceholder)
            HidePlaceholders();
    }

    public BasicCard GeneratePlaceholder(CardInfo info = null)
    {
        BasicCard newCard = Instantiate(slotPrefab, startPoint is {} ? startPoint.position : _rowTransform.position, 
            Quaternion.identity, _rowTransform).GetComponent<BasicCard>();
        newCard.parent = this;
        if (newCard.cardComponent is null)
        {
            newCard.gameObject.SetActive(false);
        }
        else
        {
            if (allCanAttackAtStart)
            {
                newCard.cardComponent.canAttack = true;
            }

            if (info is { })
            {
                newCard.cardComponent.info = info;
            }
        }
        return newCard;
    }

    public void UpdateSlotPositions()
    {
        for (int i = numCardSlots; i < cardSlots.Count; i++)
        {
            Destroy(cardSlots[i].gameObject);
            cardSlots.RemoveAt(i);
        }
        
        // Note to self: wtf?
        float startPos = -((_cardSlotWidth + cardSlotGap) * (numCardSlots - 1)) / 2f;
        Vector3 ownPos = _rowTransform.position;
        for (int cardSlotCounter = 0; cardSlotCounter < numCardSlots; cardSlotCounter++)
        {
            BasicCard cardSlot;
            if (cardSlotCounter >= cardSlots.Count)
            {
                cardSlot = GeneratePlaceholder();
                cardSlots.Add(cardSlot);
            }
            else
            {
                cardSlot = cardSlots[cardSlotCounter];
            }
            
            if (cardSlot.cardComponent is null)
            {
                cardSlot.cardTransform.localPosition = new Vector3(startPos + cardSlotCounter * (_cardSlotWidth + cardSlotGap), 0, 0);
            }
            else
            {
                cardSlot.cardComponent.side = 
                    rowSide == RowSide.Friendly || rowSide == RowSide.FriendlyHand 
                        ? GameStatus.Side.Friendly : GameStatus.Side.Enemy;
                cardSlot.cardComponent.returnPosition = ownPos +
                    new Vector3(startPos + cardSlotCounter * (_cardSlotWidth + cardSlotGap), 0, 0);
            }
        }
    }

    public void ShowPlaceholders()
    {
        if (rowSide == RowSide.FriendlyHand || rowSide == RowSide.EnemyHand) return;
        foreach(BasicCard cardSlot in cardSlots)
        {
            cardSlot.gameObject.SetActive(true);
        }
    }

    public void HidePlaceholders()
    {
        if (rowSide == RowSide.FriendlyHand || rowSide == RowSide.EnemyHand) return;
        foreach (BasicCard cardSlot in cardSlots.Where(cardSlot => cardSlot.cardComponent is null))
        {
            cardSlot.gameObject.SetActive(false);
        }
    }

    public void RemoveCard(BasicCard card)
    {
        int index = cardSlots.IndexOf(card);
        if (index < 0) return;
        
        cardSlots.RemoveAt(index);
        numCardSlots--;
        UpdateSlotPositions();

    }

    public void AddCard(BasicCard newCard, int index)
    {
        newCard.parent = this;
        newCard.cardTransform.parent = _rowTransform;
        cardSlots.Insert(index, newCard);
        numCardSlots++;
        UpdateSlotPositions();
    }
}
