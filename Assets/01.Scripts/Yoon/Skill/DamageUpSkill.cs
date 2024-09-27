using UnityEngine;

public class DamageUpSkill : WeaponSkill
{
    public override void UseSkill(Transform targetTransform, SkillData skillData)
    {
        // 추가 데미지
        bool isCheck = targetTransform.TryGetComponent(out AgentHealth enemyHealth);
        if (isCheck)
        {
            if (enemyHealth.CurrentHp > 0)
            {
                enemyHealth.OnDamage(skillData.Damage, Vector3.zero);
            }
        }
    }
}
