using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commands;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static readonly List<CardRow> FriendlyRows = new List<CardRow>(1);

    public static readonly List<CardRow> EnemyRows = new List<CardRow>(1);

    public static CardRow FriendlyHandRow;
    public static CardRow EnemyHandRow;

    public BasicCard enemyPlayer;
    public BasicCard friendlyPlayer;

    public Deck friendlyDeck;
    public Deck enemyDeck;

    public static GameManager Instance;

    public static IEnumerable<CardRow> CurrentSideRows => GameStatus.CurrentSide == GameStatus.Side.Friendly ? FriendlyRows : EnemyRows;
    public static IEnumerable<CardRow> OtherSideRows => GameStatus.CurrentSide == GameStatus.Side.Friendly ? EnemyRows : FriendlyRows;
    
    // Instance data
    
    [Tooltip("The minimum distance required for a card to snap to the placeholder")]
    public float minSnappingDistance = 3;

    [Tooltip("The amount of cards that get assigned for the first side (other side gets +1)")]
    public int startingCardCount = 3;
    [Space]
    public TMP_Text friendlyManaIndicator;
    public TMP_Text enemyManaIndicator;
    public TMP_Text currentSideIndicator;
    [Space] 
    public Transform attackIndicatorBegin;
    public Transform attackIndicatorEnd;
    
    // Command queue
    public static readonly Queue<Command> CommandQueue = new Queue<Command>();

    private void Awake()
    {
        Instance = this;
        UpdateUI();
    }

    private void Start()
    {
        GameStatus.FriendlyRemainingCards = new Queue<CardInfo>(friendlyDeck.cards);
        GameStatus.EnemyRemainingCards = new Queue<CardInfo>(enemyDeck.cards);
        
        StartCoroutine(HandleCommands());
        StartCoroutine(BeginMatch());
    }

    private static IEnumerator HandleCommands()
    {
        while (!GameStatus.MatchOver)
        {
            while (CommandQueue.Count > 0)
            {
                Command command = CommandQueue.Dequeue();
                if (!command.Validate()) continue;
                
                command.Execute();
                command.Invoke();
            }

            yield return null;
        }
    }

    private IEnumerator BeginMatch()
    {
        yield return new WaitForSeconds(1);

        for (int i = 0; i < startingCardCount; i++)
        {
            CommandQueue.Enqueue(new DrawCardCommand(GameStatus.Side.Friendly));
            CommandQueue.Enqueue(new DrawCardCommand(GameStatus.Side.Enemy));
        }
    }

    public static void RegisterCardRow(CardRow row)
    {
        switch (row.rowSide)
        {
            case CardRow.RowSide.Friendly:
                FriendlyRows.Add(row);
                break;
            case CardRow.RowSide.Enemy:
                EnemyRows.Add(row);
                break;
            case CardRow.RowSide.FriendlyHand:
                FriendlyHandRow = row;
                break;
            case CardRow.RowSide.EnemyHand:
                EnemyHandRow = row;
                break;
        }
    }

    public static BasicCard GetClosestPlaceholder(Vector3 src, GameStatus.Side side, bool searchEnemies = false)
    {
        float closest = float.PositiveInfinity;
        BasicCard closestPlaceholder = null;
        
        List<CardRow> rows = side == GameStatus.Side.Friendly ? FriendlyRows : EnemyRows;
        foreach (CardRow cardRow in rows)
        {
            foreach (BasicCard rowCardSlot in cardRow.cardSlots)
            {
                if (rowCardSlot.cardComponent == !searchEnemies) continue;
                float dist = (rowCardSlot.cardTransform.position - src).sqrMagnitude;
                if (!(dist < Instance.minSnappingDistance) || !(dist < closest)) continue;
                closest = dist;
                closestPlaceholder = rowCardSlot;
            }
        }

        return closestPlaceholder;
    }
    public void EndTurn()
    {
        CommandQueue.Enqueue(new EndTurnCommand());
    }

    public void UpdateUI()
    {
        friendlyManaIndicator.text = $"Friendly mana: {GameStatus.FriendlyMana}";
        enemyManaIndicator.text = $"Enemy mana: {GameStatus.EnemyMana}";
        currentSideIndicator.text = $"{Enum.GetName(typeof(GameStatus.Side), GameStatus.CurrentSide)}'s turn";
    }

    public static void ToggleAttackIndicator(bool state)
    {
        Instance.attackIndicatorBegin.parent.gameObject.SetActive(state);
    }
    
    public static void SetAttackIndicatorBegin(Vector3 pos)
    {
        Instance.attackIndicatorBegin.position = pos;
    }

    public static void SetAttackIndicatorEnd(Vector3 pos)
    {
        Instance.attackIndicatorEnd.position = pos;
    }

    public static void GameOver(GameStatus.Side side)
    {
        GameStatus.MatchOver = true;
        Debug.Log("Game over lol");
    }
}
