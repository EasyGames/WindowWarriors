using UnityEngine;
using System.Collections;

public class WizzardScript : HeroBase {

    public override void Awake()
    {
        myOffenseSkills.Add(new STargetEnemy_Fireball());
        base.Awake();
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
