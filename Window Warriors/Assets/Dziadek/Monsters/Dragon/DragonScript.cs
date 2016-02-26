using UnityEngine;
using System.Collections;

public class DragonScript : EntityBase {

	float previosTime;
	bool doOnce = true;
	bool giveXP = true;
	int xpReward = 2000;
	int xpPerPerson;


	void Start () {
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		if (life <= 0)
		{
			life = 0;
			if (giveXP)
			{

				drawFloatingText("2000XP", Color.blue);
				foreach(EntityBase enemy in enemiesList)
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
			animator.SetTrigger("Death");

		}

		if (doOnce)
		{
			previosTime = Time.time;
			doOnce = false;
		}

		if (Time.time - previosTime > 4.0f*10/speed && enemiesList.Count > 0 && isThereEnemy() && life > 0)
		{
			dealDamageToEnemy(strength, enemiesList[0]);
			previosTime = Time.time;
		}

	}





	// Note that this function is only meant to be called from OnGUI() functions.



}
