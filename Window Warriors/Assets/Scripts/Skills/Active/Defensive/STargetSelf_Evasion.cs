using UnityEngine;
using System.Collections;

public class STargetSelf_Evasion : MonoBehaviour {

    public float SkillCooldown = 40.0f;
    public float SkillDuration = 10.0f;
    int evasionbuff = 80;
    float timeSkillUsed;
    EntityBase user;
    public bool active = false;

    public void evasion(EntityBase userCharacter)
    {
        print("Evasion On!");
        user = userCharacter;
        userCharacter.dodgeBuff += evasionbuff;
        userCharacter.drawFloatingText("Evasion on!", Color.green);
        timeSkillUsed = Time.time;
        active = true;
    }

    public void evasionEnd(EntityBase userCharacter)
    {
        print("Evasion Off!");
        active = false;
        userCharacter.dodgeBuff -= evasionbuff;
        user.drawFloatingText("Evasion off!", Color.red);
    }


}
