using UnityEngine;
using System.Collections;
using Scripts;

public class STargetEnemy_TargetSecondPosition : SOffensive
{
    bool foundTarget = false;
    int randomTarget;

    public override void useSkill(EntityBase user)
    {
        foreach(EntityBase enemy in user.enemiesList)
        {
            if (enemy.myPosition == 1)
            {
                target = enemy;
                foundTarget = true;
                break;
            }
            else
            {
                foundTarget = false;
            }
        }
        if (!foundTarget)
        {
            randomTarget = Random.Range(0, user.enemiesList.Count);
            target = user.enemiesList[randomTarget];
        }
        user.targetEnemy.Add(target);
    }
}

