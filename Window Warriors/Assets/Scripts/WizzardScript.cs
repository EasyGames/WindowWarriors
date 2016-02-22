using UnityEngine;
using System.Collections;

public class WizzardScript : HeroBase {

    public override void Awake()
    {
        base.Awake();
    }

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
        
        Inteligence++;
        if (Level % 2 == 0)
        {
            Wisdom++;
        }
        if (Level % 3 == 0)
        {   
            Endurance++;
            maxLife = Endurance * 10;
            if(maxLife - life > 10)
            {
                life += 10;
            }
            else
            {
                life += maxLife - life;
            }

        }
        if (Level % 4 == 0)
        {
            speed++;
        }
        Level++;
        finalDMG = Inteligence;
        for (int i = 1; i < currentEquipment.Length; i++)
        {
            if (currentEquipment[i] != null)
            {
                if (currentEquipment[i].GetComponent<ItemBase>().additionalDmg > 0)
                {
                    finalDMG += currentEquipment[i].GetComponent<ItemBase>().additionalDmg;
                }
            }
        }
    }
}
