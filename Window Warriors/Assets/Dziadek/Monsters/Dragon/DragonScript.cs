using UnityEngine;
using System.Collections;

public class DragonScript : EnemyBase {

    public override void Awake()
    {
        base.Awake();
        targetingSkill = new STargetEnemy_TargetFirstPosition();
        myOffenseSkills.Add(new STargetEnemy_BasicAttack());
        giveXP = true;
        xpReward = 2000;
	}





	// Note that this function is only meant to be called from OnGUI() functions.



}
