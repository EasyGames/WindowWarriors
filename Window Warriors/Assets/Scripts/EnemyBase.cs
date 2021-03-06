﻿using UnityEngine;
using System.Collections;

public class EnemyBase : EntityBase {

    float previosTime;
    bool doOnce = true;
    public bool giveXP = true;
    public int xpReward = 50;
    int xpPerPerson;


    void Start()
    {
        animator = GetComponent<Animator>();
        drawHealthbar(true);
        animator.SetBool("Idle", true);
    }

    void Update()
    {
        updateHealthBar();
        if (life <= 0)
        {
            life = 0;
            if (giveXP)
            {

                drawFloatingText(xpReward.ToString()+"XP", Color.blue);
                foreach (EntityBase enemy in enemiesList)
                {
                    if (enemy != null)
                    {
                        xpPerPerson = xpReward / enemiesList.Count;
                        if (xpReward % enemiesList.Count == 0)
                        {
                            enemy.GetXP(xpPerPerson);
                        }
                        else
                        {
                            enemy.GetXP(xpPerPerson + xpReward % enemiesList.Count);
                            xpReward -= xpReward % enemiesList.Count;
                        }
                    }
                }
                giveXP = false;
            }
            animator.SetTrigger("Dead");

        }

        if (doOnce)
        {
            previosTime = Time.time;
            doOnce = false;
        }

        if (Time.time - previosTime > 4.0f * 10 / speed && enemiesList.Count > 0 && isThereEnemy() && life > 0)
        {
            fightEnemies();
            previosTime = Time.time;
        }

    }
}
