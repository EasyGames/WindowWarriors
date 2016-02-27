using UnityEngine;
using System.Collections;

public class Level04 : WindowBase {

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
						enemy = entityFactory.initializeBat(position + Vector3.right * 2).GetComponent<EntityBase>();
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
			else if (currentWave % 5 == 0)
			{
				if (currentWave % 25 == 0)
				{
					enemy = entityFactory.initializeRingStealer(position + Vector3.right, 50, 50, 50, 50).GetComponent<EntityBase>();
					enemy.setEnemies(herosList);
					enemiesList.Add(enemy);
				}
				else if (currentWave % 10 == 0)
				{
					enemy = entityFactory.initializeRingStealer(position + Vector3.right, 25, 25, 25, 25).GetComponent<EntityBase>();
					enemy.setEnemies(herosList);
					enemiesList.Add(enemy);
				}
				else
				{
					enemy = entityFactory.initializeRingStealer(position + Vector3.right, 15, 15, 15, 15).GetComponent<EntityBase>();
					enemy.setEnemies(herosList);
					enemiesList.Add(enemy);
				}
				if (currentState == windowState.minimized)
				{
					foreach (EntityBase boss in enemiesList)
					{
						boss.GetComponent<SpriteRenderer>().enabled = false;
						boss.drawGUI = false;
					}
				}
				sendEnemies();
				doOnce = true;
			}
			/*
			else if (currentWave == wavesToBeFinished)
			{
				//print ("teraz tutaj");
				if (Time.time - lastTime > 5.0f)
				{
					enemy = entityFactory.initializeRingStealer(position + Vector3.right, 15, 15, 15, 15).GetComponent<EntityBase>();
					enemy.setEnemies(herosList);
					enemiesList.Add(enemy);
					if (currentState == windowState.minimized)
					{
						foreach (EntityBase boss in enemiesList)
						{
							boss.GetComponent<SpriteRenderer>().enabled = false;
							boss.drawGUI = false;
						}
					}
					sendEnemies();
					doOnce = true;
				}

			}
			*/
			else
			{
				//print ("jednak tutaj");
				wavesToBeFinished = 0;
				windowCleared = true;
				reward = entityFactory.initializeChest(marker.transform.position + Vector3.up);

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

