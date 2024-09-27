using System.Collections;
using UnityEngine;

public class RowSkill : WeaponSkill
{
    public override void UseSkill(Transform targetTransform, SkillData skillData)
    {
        StartCoroutine(ThrowRow(skillData));
    }

    private IEnumerator ThrowRow(SkillData skillData)
    {
        yield return new WaitForSeconds(skillData.Duration);
        PlayerAttack playerAttack = GameManager.Instance.PlayerTrm.GetComponent<PlayerAttack>();
        playerAttack.Throw();
    }
}
