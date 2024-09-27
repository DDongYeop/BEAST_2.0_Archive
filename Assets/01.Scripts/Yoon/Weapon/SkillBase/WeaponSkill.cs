using UnityEngine;

public abstract class WeaponSkill : MonoBehaviour
{
    public abstract void UseSkill(Transform targetTransform, SkillData skillData);
}
