using UnityEngine;

public class Skill : ScriptableObject, ISkill
{
    public GameObject[] usedPrefabs;
    public virtual void Cast(Transform parent)
    {
        Debug.Log("Cast!");
    }
}
