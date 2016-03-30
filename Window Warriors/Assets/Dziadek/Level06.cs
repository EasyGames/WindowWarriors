using UnityEngine;
using System.Collections;

// This is a 1st level of the game, a village.
public class Level06 : WindowBase {
	public EntityFactory entityFactory;

	float lastTime;
	float life = 100.0f;
	int random;
	float previousTime;
	bool awardHero = true;

	public InventoryWindow inventoryWindow;


	void OnMouseUp()
	{
		print("click");
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
					if (currentWave % 5 == 0 && currentWave >=5)
					{
						if (currentWave % 25 == 0 && currentWave >=25)
						{
							enemy = entityFactory.initializeVampireKing(position + Vector3.right, 50, 50, 50, 50).GetComponent<EntityBase>();
                            spawnEssentials(enemy);
                        }
						else if (currentWave % 10 == 0  && currentWave >=10)
						{
							enemy = entityFactory.initializeVampirePrince(position + Vector3.right, 25, 25, 25, 25).GetComponent<EntityBase>();
                            spawnEssentials(enemy);
                        }
						else
						{
							enemy = entityFactory.initializeVampire(position + Vector3.right, 15, 15, 15, 15).GetComponent<EntityBase>();
                            spawnEssentials(enemy);
                        }
					}
					else if(currentWave <= 2)
					{
						LowLowLevelSpawn();
					}
					else if(currentWave <=4 && currentWave >=3)
					{
						LowLevelSpawn();
					}
					else if(currentWave <=9 && currentWave >=6)
					{
						LowMediumLevelSpawn();
					}
					else if(currentWave <=14 && currentWave >=11)
					{
						MediumLevelSpawn();
					}
					else if(currentWave <=19 && currentWave >=16)
					{
						MediumHighLevelSpawn();
					}
					else if(currentWave <=24 && currentWave >=21)
					{
						HighLevelSpawn();
					}
					else if(currentWave >=26)
					{
						HighHighLevelSpawn();
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
                    refreshEnemiesPositions();
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

	private void LowLowLevelSpawn ()
	{
		random = Random.Range (1,3);
		switch (random)
		{
		case 2:
			enemy = entityFactory.initializeZombie(position + Vector3.right).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                break;
		case 1:
			enemy = entityFactory.initializeHeadlessZombie(position + Vector3.right * 2).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                break;
		}
	}
	private void LowLevelSpawn ()
	{
		random = Random.Range (1,3);
		switch (random)
		{
		case 2:
			enemy = entityFactory.initializeZombie(position + Vector3.right).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                goto case 1;
		case 1:
			enemy = entityFactory.initializeHeadlessZombie(position + Vector3.right * 2).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                break;
		}
	}
	private void LowMediumLevelSpawn ()
	{
		switch (random)
		{
		case 3:
			enemy = entityFactory.initializeZombie(position).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                goto case 2;
		case 2:
			enemy = entityFactory.initializeZombie(position + Vector3.right).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                goto case 1;
		case 1:
			enemy = entityFactory.initializeHeadlessZombie(position + Vector3.right * 2).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                break;
		}
	}
	private void MediumLevelSpawn ()
	{
		switch (random)
		{
		case 3:
			enemy = entityFactory.initializeSkeletonArcher(position).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                goto case 2;
		case 2:
			enemy = entityFactory.initializeSkeletonWarrior(position + Vector3.right).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                goto case 1;
		case 1:
			enemy = entityFactory.initializeSkeleton(position + Vector3.right * 2).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                break;
		}
	}
	private void MediumHighLevelSpawn ()
	{
		random = Random.Range (2,4);
		switch (random)
		{
		case 3:
			enemy = entityFactory.initializeSkeletonArcher(position).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                goto case 2;
		case 2:
			enemy = entityFactory.initializeSkeletonWarrior(position + Vector3.right).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                goto case 1;
		case 1:
			enemy = entityFactory.initializeSkeleton(position + Vector3.right * 2).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                break;
		}
	}
	private void HighLevelSpawn ()
	{
		random = Random.Range (2,4);
		switch (random)
		{
		case 3:
			enemy = entityFactory.initializeNecromancer(position).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                goto case 2;
		case 2:
			enemy = entityFactory.initializeMummy(position + Vector3.right).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                goto case 1;
		case 1:
			enemy = entityFactory.initializeGhost(position + Vector3.right * 2).GetComponent<EntityBase>();
                spawnEssentials(enemy);
                break;
		}
	}
	private void HighHighLevelSpawn ()
	{
		enemy = entityFactory.initializeDarkWizard(position).GetComponent<EntityBase>();
        spawnEssentials(enemy);

        enemy = entityFactory.initializePharaoh(position + Vector3.right).GetComponent<EntityBase>();
        spawnEssentials(enemy);

        enemy = entityFactory.initializeSpirit(position + Vector3.right * 2).GetComponent<EntityBase>();
        spawnEssentials(enemy);
    }

}
