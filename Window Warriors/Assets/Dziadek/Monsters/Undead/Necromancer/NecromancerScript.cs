using UnityEngine;
using System.Collections;

public class NecromancerScript : EnemyBase
{
    public override void Awake()
    {
        base.Awake();
        targetingSkill = new STargetEnemy_TargetFirstPosition();
        myOffenseSkills.Add(new STargetEnemy_BasicAttack());
        giveXP = true;
        xpReward = 120;
    }
}