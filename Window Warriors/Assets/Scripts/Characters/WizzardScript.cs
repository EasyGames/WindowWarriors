using UnityEngine;
using System.Collections;

public class WizzardScript : HeroBase {

    STargetEnemy_Fireball fireball;
    float fireballLastUsed;

    public override void Awake()
    {
        fireball = new STargetEnemy_Fireball();
        base.Awake();
    }
    public override void fightEnemies()
    {
        if(Time.time - fireballLastUsed >= fireball.SkillCooldown)
        {
            fireball.fireball(this);
            fireballLastUsed = Time.time;
        }
        else
        {
            base.fightEnemies();
        }
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
