using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HeroBase : EntityBase
{
    float lastTime;
    public enum state { Idle, Walking, Dead };
    public state currentState;

    //Enumerator of Hero menu status
    //public enum menuState {InMenu, InAction, Locked}
    //public menuState currentMenuState;

    

    // 0 - Hero icon slot
    // 1 - Helmet slot
    // 2 - Armor slot 
    // 3 - shoes slot
    // 4 - right hand slot
    // 5 - left hand slot
    public GameObject[] currentEquipment;

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        currentState = state.Walking;
    }

    public void Start()
    {
        drawHealthbar(false);
    }

    public override void recalculateDMG()
    {
        base.recalculateDMG();
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

    public override void FixedUpdate()
    {
        updateHealthBar();
        base.FixedUpdate();
        if (enemiesList.Count > 0 && currentState != state.Idle && currentState != state.Dead)
        {
            print("Enter Idle");
            currentState = state.Idle;
        }
        if (enemiesList.Count > 0 && currentState == state.Idle && isThereEnemy() && Time.time - lastTime > 4.0f*10/speed)
        {
            animator.SetTrigger("Attack");
            lastTime = Time.time;
        }
        if (enemiesList.Count == 0 && currentState != state.Walking && currentState != state.Dead)
        {
            currentState = state.Walking;
        }
        if (currentState == state.Walking)
        {
            walking = true;
            animator.SetBool("Idle", false);
        }
        if (currentState == state.Idle)
        {
            walking = false;
            animator.SetBool("Idle", true);
        }

        if (life <= 0)
        {
            life = 0;
            currentState = state.Dead;
            animator.SetBool("Dead", true);
        }
        else
        {
            currentState = state.Idle;
            animator.SetBool("Dead", false);
        }

    }

}