using UnityEngine;
using System.Collections;

public class RougeScript : HeroBase {

    STargetSelf_Evasion evasion;
    float evasionLastUsed = -39.0f;

    public override void Awake()
    {
        evasion = new STargetSelf_Evasion();
        base.Awake();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Time.time - evasionLastUsed >= evasion.SkillCooldown && isThereEnemy())
        {
            evasion.evasion(this);
            evasionLastUsed = Time.time;
        }
        if (evasion.active && Time.time - evasionLastUsed >= evasion.SkillDuration)
        {
            evasion.evasionEnd(this);
        }
    }

    public override void LevelUp()
    {
        agility++;
        if (Level % 2 == 0)
        {
            endurance++;
        }
        if (Level % 3 == 0)
        {
            speed++;
        }
        if (Level % 4 == 0)
        {
            charisma++;
        }
        base.LevelUp();
    }
}
