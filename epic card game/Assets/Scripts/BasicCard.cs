using JetBrains.Annotations;
using UnityEngine;

public class BasicCard : MonoBehaviour
{
    public Transform cardTransform;
    
    [CanBeNull]
    public Card cardComponent;

    public CardRow parent;
}
