using UnityEngine;
using System.Collections;
using Scripts;

public class STargetEnemy_Fireball : SOffensive
{

    public STargetEnemy_Fireball()
    {
        mySkillMechanic = SkillMechanic.AoE;
        SkillCooldown = 30.0f;
        tier = 1;
        skillLevel = 2;
    }

    public override void useSkill(EntityBase user)
    {
        user.animator.SetTrigger("Attack");
        user.animator.SetFloat("speed", user.speed / 10);
        user.drawFloatingText("Fireball!", Color.yellow);
        foreach (EntityBase enemy in user.enemiesList)
        {
            user.targetEnemy.Add(enemy);
        }
        user.DamageToDeal = user.finalDMG * 3;
        base.useSkill(user);
    }

}