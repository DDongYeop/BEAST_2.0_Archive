using UnityEngine;
using DG.Tweening;

public class ScaleUpSkill : WeaponSkill
{
    public override void UseSkill(Transform targetTransform, SkillData skillData)
    {
        transform.DOScale(transform.localScale * 1.8f, skillData.Duration);
    }
}
