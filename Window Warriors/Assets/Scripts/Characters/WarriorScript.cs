using UnityEngine;
using System.Collections;

public class WarriorScript : HeroBase {

    public AnimationClip cutEffect;

    public override void Awake()
    {
        base.Awake(); 
    }

    public override void LevelUp()
    {
        strength++;
        if (Level % 2 == 0)
        {
            endurance++;
        }
        if (Level % 3 == 0)
        {
            speed++;
        }
        if (Level % 4 == 0)
        {
            charisma++;
        }
        base.LevelUp();
    }

    public override void dealDamageToEnemy(int Dmg, EntityBase _entityScript)
    {
        base.dealDamageToEnemy(Dmg, _entityScript);
       // animator.Play("Slash");
    }
}
