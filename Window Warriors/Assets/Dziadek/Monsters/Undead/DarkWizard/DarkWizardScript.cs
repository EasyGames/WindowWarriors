using UnityEngine;
using System.Collections;

public class DarkWizardScript : EnemyBase {

    public override void Awake()
    {
        base.Awake();
        targetingSkill = new STargetEnemy_TargetFirstPosition();
        myOffenseSkills.Add(new STargetEnemy_BasicAttack());
        myOffenseSkills.Add(new STargetEnemy_Fireball());
        giveXP = true;
        xpReward = 200;
    }
}
