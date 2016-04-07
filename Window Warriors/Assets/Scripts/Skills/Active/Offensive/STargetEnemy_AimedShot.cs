using UnityEngine;
using System.Collections.Generic;

public class STargetEnemy_AimedShot : SOffensive
{
    public STargetEnemy_AimedShot()
    {
        mySkillMechanic = SkillMechanic.Single;
        SkillCooldown = 20.0f;

    }
    public override void useSkill(EntityBase user)
    {  
        user.dealDamageToEnemy(user.finalDMG*2,user.enemiesList[0]);
        user.drawFloatingText("AIMED SHOOT!", Color.blue);
        base.useSkill(user);
    }
}
