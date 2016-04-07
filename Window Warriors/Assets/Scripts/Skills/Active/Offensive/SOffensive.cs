using UnityEngine;
using System.Collections;

public class SOffensive : MonoBehaviour {

    public enum SkillMechanic {Single, AoE, DoT, Buff, Debuff, Heal};
    public SkillMechanic mySkillMechanic;

    public float SkillCooldown;
    public float SkillLastUsed;
    
    public virtual void useSkill(EntityBase user)
    {
        SkillLastUsed = Time.time;
    }

}
