using System;
using System.Collections.Generic;

public static class GameStatus
{
    public static Queue<CardInfo> FriendlyRemainingCards;
    public static Queue<CardInfo> EnemyRemainingCards;
    public static int FriendlyMana = 1;
    public static int EnemyMana = 1;
    public static int RoundCount = 1;

    public enum Side
    {
        Friendly,
        Enemy
    }

    public static Side CurrentSide = Side.Friendly;
    public static Side WentFirst = Side.Friendly;
    public static Side OtherSide => CurrentSide == Side.Friendly ? Side.Enemy : Side.Friendly;

    public static int CurrentSideMana
    {
        get => CurrentSide == Side.Friendly ? FriendlyMana : EnemyMana;
        set
        {
            switch (CurrentSide)
            {
                case Side.Friendly:
                    FriendlyMana = value;
                    break;
                case Side.Enemy:
                    EnemyMana = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public static bool MatchOver = false;
}
