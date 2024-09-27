using UnityEngine;

public class DecelerationSkill : WeaponSkill
{
    public override void UseSkill(Transform targetTransform, SkillData skillData)
    {
        if (targetTransform.TryGetComponent(out EnemyBrain enemyBrain))
        {
            enemyBrain.SetSlow(skillData.Duration, 60f);
        }
    }
}
