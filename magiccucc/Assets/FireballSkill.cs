using UnityEngine;

[CreateAssetMenu]
public class FireballSkill : Skill
{
    public override void Cast(Transform parent)
    {
        Instantiate(usedPrefabs[0], parent.position, parent.rotation);
    }
}
