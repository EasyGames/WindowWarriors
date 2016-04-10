using UnityEngine;
using System.Collections;
using Scripts;

public class STargetEnemy_TargetFirstPosition : SOffensive
{
    public override void useSkill(EntityBase user)
    {
        target = user.enemiesList[0];
        user.targetEnemy.Add(target);
    }
}

