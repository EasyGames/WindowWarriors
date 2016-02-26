using UnityEngine;
using System.Collections;

public class ArcherScript : HeroBase {

    bool fighting = false;
    public GameObject arrowGO;
    GameObject thisArrow;
    public float arrowSpeed;
    bool targetOnce = true;
    Vector3 targetForArrow;

    public override void Awake()
    {
        base.Awake();
        arrowDelegate = destroyArrow;
    }

    void shootArrow()
    {
        if (isThereEnemy())
        {
            targetForArrow = enemiesList[0].transform.position + Vector3.up * 0.4f;
            fightEnemies();
            destroyArrow();
            thisArrow = (GameObject)Instantiate(arrowGO, transform.position + Vector3.right * 2.0f + Vector3.up * 0.4f, Quaternion.identity);
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
                thisArrow.transform.position += Vector3.right * Time.deltaTime * arrowSpeed * speed/10;
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
