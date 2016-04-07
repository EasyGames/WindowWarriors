using UnityEngine;
using System.Collections;

public class STargetEnemy_Fireball : SOffensive {

    public STargetEnemy_Fireball()
    {
        mySkillMechanic = SkillMechanic.AoE;
        SkillCooldown = 30.0f;
    }

    public override void useSkill(EntityBase user)
    {
        user.drawFloatingText("Fireball!", Color.yellow);
        foreach (EntityBase enemy in user.enemiesList)
        {
            enemy.takeDamageFromEnemy(user.finalDMG * 3);
        }
        base.useSkill(user);
    }
}
