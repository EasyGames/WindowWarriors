using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : WindowBase {

    public EntityFactory entityFactory;

    
    float lastTime;
    float life = 100.0f;
    int random;
    float previousTime;

    bool awardHero = true;

    void OnMouseUp()
    {
        if (windowCleared)
        {
            if ((addWaves - 1) % 1 == 0 && addWaves >= 1)
            {
                heroMenu.unlockHero("Tomo","wizzard");
            }

            if ((addWaves - 1) % 10 == 0 && addWaves >= 10)
            {
                heroMenu.unlockHero("Willy","archer");
            }
            if (herosList.Count > 0)
            {
                currentWave = 1;
            }
            else
            {
                currentWave = 0;
            }
            reward.GetComponent<Animator>().SetBool("openChest", true);
            reward.GetComponent<EntityBase>().drawFloatingText("+" + rewardAmount+"g", Color.yellow);
            moneyManager.addGold(rewardAmount);
            lastTime = Time.time;
            wavesToBeFinished = addWaves;
            addWaves++;
            windowCleared = false;

            if (awardHero)
            {
                heroMenu.unlockHero("Ragnar", "archer");
                awardHero = false;
            }
        }
    }

    public override void Start()
    {
        base.Start();
        wavesToBeFinished = 1;
        position = transform.position + Vector3.right - Vector3.up * 0.8f;
        windowCleared = false;
        currentWave = 0;
        rewardAmount = 50;
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!isThereEnemy() && herosList.Count > 0 && isThereHeroAlive() && !windowCleared && !isHeroResting)
        {
            if (doOnce)
            {
                random = Random.Range(1, 4);
                doOnce = false;
                lastTime = Time.time;
                currentWave++;
            }
            if (currentWave <= wavesToBeFinished)
            {

                if (Time.time - lastTime > 5.0f)
                {
                    position = new Vector3(transform.position.x + 1 *ratio, transform.position.y - 1 * 0.8f*ratio,(ratio> 1)? -1.1f:0);
                    switch (random)
                    {
                        case 3:
                            enemy = entityFactory.initializeSlime(position).GetComponent<EntityBase>();
                            spawnEssentials(enemy);
                            goto case 2;
                        case 2:
                            enemy = entityFactory.initializeSlime(position + Vector3.right*ratio).GetComponent<EntityBase>();
                            spawnEssentials(enemy);
                            goto case 1;
                        case 1:
                            enemy = entityFactory.initializeSlime(position + Vector3.right * 2 *ratio).GetComponent<EntityBase>();
                            spawnEssentials(enemy);
                            break;
                    }
                    if (currentState == windowState.minimized)
                    {
                        foreach (EntityBase enemy in enemiesList)
                        {
                            enemy.GetComponent<SpriteRenderer>().enabled = false;
                            enemy.drawGUI = false;
                        }
                    }
                    refreshEnemiesPositions();
                    sendEnemies();
                    doOnce = true;
                }
            }
            else
            {
                Vector3 rewardPosition;
                if (currentState == windowState.fullScreen)
                {
                    rewardPosition = new Vector3(transform.position.x, transform.position.y - 1 * ratio, (ratio > 1) ? -1.1f : 0);
                }
                else
                {
                    rewardPosition = transform.parent.position + Vector3.up;
                }
                wavesToBeFinished = 0;
                windowCleared = true;
                reward = entityFactory.initializeChest(rewardPosition);
                EntityBase rewardScript = reward.GetComponent<EntityBase>();
                rewardScript.transform.localScale = new Vector3(rewardScript.transform.localScale.x * ratio, rewardScript.transform.localScale.y * ratio, rewardScript.transform.localScale.z);
                rewardScript.currentWindow = this;
                rewardScript.setWorldPos();
                rewardScript.currentWindow = this;
                
            }
        }


        if (Time.time - previousTime > 5.0f && windowCleared)
        {
            foreach (EntityBase hero in herosList)
            {
                if (hero.life < hero.maxLife)
                {
                    hero.life += 5;
                    if (hero.life > hero.maxLife)
                    {
                        hero.life = hero.maxLife;
                    }
                }
            }
            previousTime = Time.time;
        }
    }



}
