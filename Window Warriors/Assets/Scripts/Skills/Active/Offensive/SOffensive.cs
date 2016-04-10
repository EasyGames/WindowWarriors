using UnityEngine;
using System.Collections;

namespace Scripts
{
    public class SOffensive
    {

        public enum SkillMechanic { Single, AoE, DoT, Buff, Debuff, Heal };
        public SkillMechanic mySkillMechanic;

        public float SkillCooldown;
        public float SkillLastUsed;
        public int tier;
        public int skillLevel;
        public EntityBase target;
        public int damageToDeal;

        public virtual void useSkill(EntityBase user)
        {
            SkillLastUsed = Time.time;
        }

    }

}
