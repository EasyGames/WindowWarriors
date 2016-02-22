using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EntityFactory : MonoBehaviour {

    // prefab list, add new prefab for each new monster
    public GameObject SlimePrefab;
    public GameObject ChestPrefab;
    
    // initialize function, coppy for each next prefab, but change the variable names and get right script
    public GameObject initializeSlime(Vector3 pos, int strenght = 0, int agility = 0, int speed =0, int endurance =0, int charisma=0, int inteligence =0, int wisdom =0)
    {
        GameObject initializedSlime;
        Slime slimeScript;
        initializedSlime = (GameObject)Instantiate(SlimePrefab, pos, Quaternion.identity);
        slimeScript = initializedSlime.GetComponent<Slime>();
        slimeScript.initialize(strenght, agility, speed, endurance, charisma, inteligence, wisdom);

        return initializedSlime;
    }

    public GameObject initializeChest(Vector3 pos)
    {
        GameObject initializedChest;
        EntityBase chestScript;
        initializedChest = (GameObject)Instantiate(ChestPrefab, pos, Quaternion.identity);
        chestScript = initializedChest.GetComponent<EntityBase>();
        chestScript.life = 0;
        return initializedChest;
    }
}
