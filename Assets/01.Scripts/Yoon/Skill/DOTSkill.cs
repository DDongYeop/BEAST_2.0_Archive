using System.Collections;
using UnityEngine;

// DOT: Damage Over Time (도트 데미지)
public class DOTSkill : WeaponSkill
{
    private readonly float checkDamageCoolTime = 0.6f;

    public override void UseSkill(Transform targetTransform, SkillData skillData)
    {
        StartCoroutine(CheckDamageCor(targetTransform, skillData.Damage, skillData.Duration));
    }

    private IEnumerator CheckDamageCor(Transform taretTransform, int damage, float durationTime = 0f)
    {
        float startTime = Time.time;
        float time = 0f;

        while (time <= durationTime)
        {
            bool isCheck = taretTransform.TryGetComponent(out AgentHealth enemyHealth);
            if (isCheck)
            {
                if (enemyHealth.CurrentHp > 0)
                {
                    enemyHealth.OnDamage(damage, Vector3.zero);
                }
            }

            time = Time.time - startTime;
            yield return new WaitForSeconds(checkDamageCoolTime);
        }
    }
}
