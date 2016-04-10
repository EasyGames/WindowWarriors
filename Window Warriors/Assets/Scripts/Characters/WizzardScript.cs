using UnityEngine;
using System.Collections.Generic;
using Scripts;

public class WizzardScript : HeroBase {

    public override void Awake()
    {
        base.Awake();
        targetingSkill = new STargetEnemy_TargetFirstPosition();
        myOffenseSkills.Add(new STargetEnemy_BasicAttack());
        myOffenseSkills.Add(new STargetEnemy_Fireball());
    }

    public override void LevelUp()
    {
        
        inteligence++;
        if (Level % 2 == 0)
        {
            wisdom++;
        }
        if (Level % 3 == 0)
        {   
            endurance++;
        }
        if (Level % 4 == 0)
        {
            speed++;
        }
        base.LevelUp();
    }
}
