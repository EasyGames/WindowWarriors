using UnityEngine;
using System.Collections.Generic;

namespace Scripts
{
    public class STargetEnemy_AimedShot : SOffensive
    {
        public STargetEnemy_AimedShot()
        {
            mySkillMechanic = SkillMechanic.Single;
            SkillCooldown = 15.0f;
            tier = 1;
            skillLevel = 1;

        }
        public override void useSkill(EntityBase user)
        {
            user.animator.SetTrigger("Attack");
            user.animator.SetFloat("speed", user.speed / 10);
            user.drawFloatingText("AIMED SHOOT!", Color.blue);
            user.DamageToDeal = user.finalDMG*2;
            base.useSkill(user); 
        }
    }
}
