using UnityEngine;
using System.Collections.Generic;

public class STargetEnemy_AimedShot : MonoBehaviour
{
    public float SkillCooldown = 20.0f;

    public void aimedShotAtFirstEnemy(EntityBase userCharacter)
    {
        print("Aimed Shoot!");
        userCharacter.finalDMG *= 2;
        userCharacter.drawFloatingText("AIMED SHOOT!", Color.blue);
        print(userCharacter.finalDMG);
    }
}
