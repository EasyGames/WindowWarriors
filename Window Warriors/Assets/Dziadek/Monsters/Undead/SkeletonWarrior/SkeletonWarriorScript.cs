using UnityEngine;
using System.Collections;

public class SkeletonWarriorScript : EnemyBase
{
    public override void Awake()
    {
        base.Awake();
        targetingSkill = new STargetEnemy_TargetFirstPosition();
        myOffenseSkills.Add(new STargetEnemy_BasicAttack());
        giveXP = true;
        xpReward = 80;
    }
}