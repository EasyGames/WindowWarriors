using UnityEngine;
using System.Collections;

// This is a 1st level of the game, a village.
public class Level01 : WindowBase {
    public EntityFactory entityFactory;

    float lastTime;
    float life = 100.0f;
    int random;
    float previousTime;
    bool awardHero = true;

    public InventoryWindow inventoryWindow;


    void OnMouseUp()
    {
        if (!inventoryWindow.isInventoryFull())
        {
            GameObject inventorySlot;
            inventorySlot = inventoryWindow.firstEmptyInventorySlot();
            if (inventorySlot != null)
            {
                inventorySlot.GetComponent<ItemSlotScript>().receiveItem(reward);
            }
        }
        else
        {
            inventoryWindow.addInventoryRow();
            GameObject inventorySlot;
            inventorySlot = inventoryWindow.firstEmptyInventorySlot();
            if (inventorySlot != null)
            {
                inventorySlot.GetComponent<ItemSlotScript>().receiveItem(reward);
            }
        }
    }

    public override void Start()
    {
        base.Start();
        wavesToBeFinished = 1;
        position = transform.position + Vector3.right - Vector3.up * 0.8f;
        windowCleared = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(windowCleared && Time.time - lastRaidTime >= raidTime)
        {
            windowCleared = false;
            if (herosList.Count > 0)
            {
                currentWave = 1;
            }
            else
            {
                currentWave = 0;
            }
            wavesToBeFinished = addWaves;
            addWaves++;
            showTimer = false;
        }

        if (!isThereEnemy() && herosList.Count > 0 && herosList[0].life > 0 && !windowCleared && !isHeroResting)
        {
            if (doOnce)
            {
                random = Random.Range(1, 2);
                doOnce = false;
                lastTime = Time.time;
                currentWave++;
            }
            if (currentWave <= wavesToBeFinished)
            { 

                if (Time.time - lastTime > 5.0f)
                {
                    switch (random)
                    {
                        case 3:
                            enemy = entityFactory.initializeSlime(position).GetComponent<EntityBase>();
                            enemy.setEnemies(herosList);
                            enemiesList.Add(enemy);
                            goto case 2;
                        case 2:
                            enemy = entityFactory.initializeSlime(position + Vector3.right).GetComponent<EntityBase>();
                            enemy.setEnemies(herosList);
                            enemiesList.Add(enemy);
                            goto case 1;
                        case 1:
                            enemy = entityFactory.initializeSlime(position + Vector3.right * 2).GetComponent<EntityBase>();
                            enemy.setEnemies(herosList);
                            enemiesList.Add(enemy);
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
                    sendEnemies();
                    doOnce = true;
                }
            }
            else
            {
                wavesToBeFinished = 0;
                windowCleared = true;
                lastRaidTime = Time.time;
                raidTime = Random.Range(30.0f, 61.0f);
                print("to next raid: " + raidTime);
                showTimer = true;
                if ((addWaves - 1) % 10 == 0 && addWaves >= 10)
                {
                    heroMenu.unlockHero("Legolas", "archer");
                }

                if ((addWaves - 1) % 15 == 0 && addWaves >= 15)
                {
                    heroMenu.unlockHero("Maho", "wizzard");
                }

                if (awardHero)
                {
                    heroMenu.unlockHero("Trollo", "warrior");
                    awardHero = false;
                }
            }
        }


        if (Time.time - previousTime > 5.0f && windowCleared)
        {
            foreach (EntityBase hero in herosList)
            {
                if (hero.life < hero.maxLife)
                {
                    hero.life += 20;
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
