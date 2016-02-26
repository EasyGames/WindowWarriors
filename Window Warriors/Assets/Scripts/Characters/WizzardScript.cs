using UnityEngine;
using System.Collections;

public class WizzardScript : HeroBase {

        /* This code is left for special ability
        * 
        foreach (EntityBase enemy in enemiesList)
        {
            if(enemy.GetComponent<EntityBase>().life > 0)
                enemy.takeDamageFromEnemy(Dmg);
        }
        */

    public override void LevelUp()
    {
        
        inteligence++;
        if (Level % 2 == 0)
        {
            wisdom++;
        }
        if (Level % 3 == 0)
        {   
            endurance++;
        }
        if (Level % 4 == 0)
        {
            speed++;
        }
        base.LevelUp();
    }
}
