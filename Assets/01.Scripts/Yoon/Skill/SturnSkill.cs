using DG.Tweening;
using UnityEngine;

public class SturnSkill : WeaponSkill
{
    public override void UseSkill(Transform targetTransform, SkillData skillData)
    {
        SturnTarget(targetTransform, skillData.Duration);
    }

    public void SturnTarget(Transform targetTransform, float sturnTime)
    {
        if (targetTransform.root.TryGetComponent(out AgentHealth enemyHealth))
        {
            enemyHealth.Stun(sturnTime);
        }
    }

}
