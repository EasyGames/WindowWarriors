using UnityEngine;
using System.Collections;

public class STargetEnemy_Fireball : MonoBehaviour {

    public float SkillCooldown = 30.0f;

    public void fireball(EntityBase userCharacter)
    {
        print("Fireball!");
        userCharacter.drawFloatingText("Fireball!", Color.yellow);
        foreach(EntityBase enemy in userCharacter.enemiesList)
        {
            enemy.takeDamageFromEnemy(userCharacter.finalDMG *3);
        }
        print(userCharacter.finalDMG);
    }
}
