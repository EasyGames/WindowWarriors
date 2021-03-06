﻿using UnityEngine;
using Scripts;
using System.Collections.Generic;

public class ArcherScript : HeroBase {

    public STargetEnemy_AimedShot aimedShoot;
    float ratio;
    bool fighting = false;
    public GameObject arrowGO;
    GameObject thisArrow;
    public float arrowSpeed;
    Vector3 targetForArrow;

    public override void Awake()
    {
        base.Awake();
        targetingSkill = new STargetEnemy_TargetSecondPosition();
        myOffenseSkills.Add(new STargetEnemy_BasicAttack());
        myOffenseSkills.Add(new STargetEnemy_AimedShot());
        arrowDelegate = destroyArrow;
    }

    void shootArrow()
    {
        if (isThereEnemy())
        {
            ratio = transform.parent.localScale.x;
            targetForArrow = enemiesList[0].transform.position + Vector3.up * 0.4f;
            destroyArrow();
            thisArrow = (GameObject)Instantiate(arrowGO, transform.position + Vector3.right * 2.0f*ratio + Vector3.up * 0.4f*ratio, Quaternion.identity);
            thisArrow.transform.localScale = new Vector3(thisArrow.transform.localScale.x * ratio, thisArrow.transform.localScale.y * ratio, thisArrow.transform.localScale.z);
            if (!drawGUI)
            {
                thisArrow.GetComponent<SpriteRenderer>().enabled = false;
            }
            fighting = true;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.speed = 1 * speed/10;
        }
        else
        {
            animator.speed = 1;
        }

        if (fighting && thisArrow != null)
        {
            if (drawGUI)
            {
                thisArrow.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                thisArrow.GetComponent<SpriteRenderer>().enabled = false;
            }
            if (thisArrow.transform.position.x <= targetForArrow.x)
            {
                thisArrow.transform.position += Vector3.right * Time.deltaTime * arrowSpeed * speed/10*ratio;
            }
            else
            {
                fighting = false;
                destroyArrow();
            }
        }
    }

    public override void LevelUp()
    {
        
        agility++;
        if (Level%2 == 0)
        {
            strength++;
        }
        if (Level%3 == 0)
        {
            speed++;
        }
        if (Level%4 == 0)
        {
            endurance++;
        }
        base.LevelUp();
    }

    public void destroyArrow()
    {
        Destroy(thisArrow);
    }
}
