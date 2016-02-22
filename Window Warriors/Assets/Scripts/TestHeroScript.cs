﻿using UnityEngine;
using System.Collections;

public class TestHeroScript : HeroBase {

    public AnimationClip cutEffect;

    public override void Awake()
    {
        base.Awake(); 
    }

    public override void LevelUp()
    {
        
        strength++;
        if (Level % 2 == 0)
        {
            Endurance++;
            maxLife = Endurance * 10;
            if (maxLife - life > 10)
            {
                life += 10;
            }
            else
            {
                life += maxLife - life;
            }
        }
        if (Level % 3 == 0)
        {
            speed++;
        }
        if (Level % 4 == 0)
        {
            Charisma++;
        }
        base.LevelUp();
    }

    public override void dealDamageToEnemy(int Dmg, EntityBase _entityScript)
    {
        base.dealDamageToEnemy(Dmg, _entityScript);
       // animator.Play("Slash");
    }
}