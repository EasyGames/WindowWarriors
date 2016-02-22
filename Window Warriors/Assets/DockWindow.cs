using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DockWindow : WindowBase {

    float previousTime;
    public void Awake()
    {
        herosList = new List<EntityBase>();
        enemiesList = new List<EntityBase>();
        maxNumberOfHeros = 30;
    }
    public override void Start()
    {
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (Time.time - previousTime > 5.0f)
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
        

    public override void addHero(EntityBase hero)
    {
        if (hero != null && !herosList.Contains(hero))
        {
            herosList.Add(hero);
        }

        for (int i = 0; i < herosList.Count; i++)
        {
            herosList[i].gameObject.transform.parent.position = transform.position - Vector3.right * i*1.2f - Vector3.right * 3 + Vector3.up * 5;
        }
    }

    public override void removeHero(EntityBase hero)
    {
        if (hero != null)
        {
            herosList.Remove(hero);
        }
        for (int i = 0; i < herosList.Count; i++)
        {
            herosList[i].gameObject.transform.parent.position = transform.position - Vector3.right * i * 1.2f - Vector3.right * 3 + Vector3.up * 5;
        }
    }
}

