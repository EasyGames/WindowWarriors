using UnityEngine;
using System.Collections;

public class WarriorScript : HeroBase {

    public AnimationClip cutEffect;

    public override void Awake()
    {
        base.Awake();
        targetingSkill = new STargetEnemy_TargetFirstPosition();
        myOffenseSkills.Add(new STargetEnemy_BasicAttack());
    }

    public override void LevelUp()
    {
        strength++;
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
