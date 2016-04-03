using UnityEngine;
using System.Collections;

public class DragonScript : EnemyBase {

    public override void Awake()
    {
        base.Awake();
        giveXP = true;
        xpReward = 2000;
	}





	// Note that this function is only meant to be called from OnGUI() functions.



}
