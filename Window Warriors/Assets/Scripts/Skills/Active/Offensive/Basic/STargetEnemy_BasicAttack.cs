using UnityEngine;
using System.Collections;
using Scripts;

public class STargetEnemy_BasicAttack : SOffensive {

    public STargetEnemy_BasicAttack()
    {
        SkillCooldown = 0.0f;
        tier = 0;
        skillLevel = 1;
    }

    public override void useSkill(EntityBase user)
    {
        user.animator.SetTrigger("Attack");
        user.animator.SetFloat("speed", user.speed / 10);
        int x;
        x = Random.Range(1, 101);
        target = user.targetEnemy[0];
            user.criticalChance = (user.agility >= target.agility * 2) ? 16 : 8 * user.agility / target.agility;
            if (user.criticalChance >= x)
            {
                user.DamageToDeal = (int)(user.finalDMG * 1.5f);
            }
            else
            {
                user.DamageToDeal = user.finalDMG;
            }
        }
}
