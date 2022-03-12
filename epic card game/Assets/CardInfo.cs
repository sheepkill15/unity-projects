using System;
using UnityEngine;

[Serializable]
public struct CardStats
{
    public int cost;
    public int health;
    public int damage;
}

[CreateAssetMenu]
public class CardInfo : ScriptableObject
{
    public enum Type
    {
        Entity,
        Spell
    }
    
    [Header("Looks")]
    public string cardName;
    public string description;

    public Sprite icon;

    [Header("Data")] 
    public CardStats stats;
    
    [Space]

    public Type type;
    
    public Effect[] effects;
}
