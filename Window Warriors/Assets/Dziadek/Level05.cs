﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level05 : WindowBase {

	public EntityFactory entityFactory;


	float lastTime;
	int random;
	float previousTime;

	bool awardHero = true;

	void OnMouseUp()
	{
		if (windowCleared)
		{
			if ((addWaves - 1) % 6 == 0 && addWaves >= 6)
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
				heroMenu.unlockHero("Olaf", "monk");
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
			if (currentWave < wavesToBeFinished)
			{
				//print ("najpierw tutaj");
				if (Time.time - lastTime > 5.0f)
				{
					enemy = entityFactory.initializeDragon(position, 30 + wavesToBeFinished*2, 20 + wavesToBeFinished*2, 30 + wavesToBeFinished*2, 30 + wavesToBeFinished*2).GetComponent<EntityBase>();
                    spawnEssentials(enemy);

                    if (currentState == windowState.minimized)
					{
						foreach (EntityBase boss in enemiesList)
						{
							boss.GetComponent<SpriteRenderer>().enabled = false;
							boss.drawGUI = false;
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

