using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Scripts;

//Hero class
public enum heroClass { All, Warrior, Archer, Mage, Monk, Rogue };

public class EntityBase : MonoBehaviour {

    //Hero class
    public heroClass myClass;
    public enum MainAtribiute {Strength, Agility, Inteligence, Wisdom, Charisma, Speed };
    public MainAtribiute mainAttribiute;

    public int finalDMG;
    public int DamageToDeal;
    public List<EntityBase> targetEnemy;
    public int myPosition;

    // Archer arrow delegate
    public delegate void ArrowDelegate();
    public ArrowDelegate arrowDelegate;

    //Tile size
    public int tileSize { get; set; }

    // Animation
    public Animator animator { get; set; }
    public bool walking { get; set; }
    public float animationMaxDuration { get; set; }
    public float actionSpeed;

    // list of enemies
    public List<EntityBase> enemiesList = new List<EntityBase>();
    // Entitybase script
    EntityBase enemyScript;

    // Health bar
    public bool right { get; set; }
    public GameObject healthBar { get; set; }
    public Color orange { get; set; }
    public Vector3 targetPos { get; set; }
    public Vector3 worldPos { get; set; }
    public int lifeHeight { get; set; }
    public int lifeWidth { get; set; }
    public bool drawGUI { get; set; }
    public WindowBase currentWindow;

    //Base Stats
    public int Level = 1;
    public int XP { get; set; }
    public int XPTreshold { get; set; }

    // Statistics
    public float life;
    public float maxLife;
    public int strength = 10;
    public int speed = 10;
    public int agility= 10;
    public int endurance= 10;
    public int charisma = 10;
    public int inteligence = 10;
    public int wisdom = 10;
    public int criticalChance { get; set; }
    public int criticalBuff { get; set; }
    public int dodgeChance { get; set; }
    public int dodgeBuff { get; set; }

    // Floating Text
    public GameObject textGameObject { get; set; }
    TextMesh floatingText { get; set; }
    RectTransform canvasTransform { get; set; }
    List<GameObject> floatingTextsGO { get; set; }

    public List<SOffensive> myOffenseSkills;
    public SOffensive targetingSkill;

    public virtual void Awake()
    {
        tileSize = 16;
        orange = new Color(200.0f / 255.0f, 130.0f / 255.0f, 40.0f / 255.0f, 1);
        drawGUI = true;
        XP = 0;
        floatingTextsGO = new List<GameObject>();
        life = endurance * 10;
        maxLife = life;
        XPTreshold = (Level == 1) ? 100 : (100 * (2 ^ (Level - 1)));
        targetEnemy = new List<EntityBase>();
        recalculations();
        updateSpeeds();
        myOffenseSkills = new List<SOffensive>();
    }

    public void updateSpeeds()
    {
        actionSpeed = 4.0f * 10 / speed;

        if (actionSpeed/2 > 2.0f)
        {
            animationMaxDuration = 2.0f;
        }
        else
        {
            animationMaxDuration = actionSpeed;
        }
        


    }

    public virtual void drawHealthbar(bool healthBarOnTheRight)
    {
        Texture2D health = new Texture2D(10, 100);
        for (int y =0; y < health.height; y++)
        {
            for (int x=0; x < health.width; x++)
            {
                health.SetPixel(x, y, Color.white);
            }
        }
        health.filterMode = FilterMode.Point;
        health.Apply();
        right = healthBarOnTheRight;
        healthBar = new GameObject();
        healthBar.name = "Health Bar";
        healthBar.transform.position = (right) ? transform.position + (Vector3.right * 1.1f) : transform.position - (Vector3.right * 0.2f);
        healthBar.AddComponent<SpriteRenderer>().color = Color.green;
        healthBar.GetComponent<SpriteRenderer>().sprite = Sprite.Create(health, new Rect(0, 0, 10, 100), new Vector2(0, 0));
        healthBar.transform.parent = transform;
    }

    public virtual void updateHealthBar()
    {
        if (currentWindow != null && drawGUI == true)
        {
            healthBar.GetComponent<SpriteRenderer>().enabled = true;
            healthBar.transform.position = (right) ? transform.position + (Vector3.right * 1.1f * currentWindow.ratio) : transform.position - (Vector3.right * 0.2f * currentWindow.ratio);
            healthBar.transform.localScale = new Vector3(1.0f , 1.0f *  (life / maxLife), 1.0f);
            if (life > maxLife / 2)
            {
                healthBar.GetComponent<SpriteRenderer>().color = Color.green;
            }
            else if (life > maxLife / 5)
            {
                healthBar.GetComponent<SpriteRenderer>().color = orange;
            }
            else
            {
                healthBar.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        else
        {
            healthBar.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public virtual void FixedUpdate()
    {

        for (int i =0; i<floatingTextsGO.Count;i++)
        {
            if (drawGUI)
            {
                floatingTextsGO[i].GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                floatingTextsGO[i].GetComponent<MeshRenderer>().enabled = false;
            }
            floatingTextsGO[i].transform.position += Vector3.up * 0.02f;
            if (floatingTextsGO[i].transform.position.y >= transform.position.y + 1.8f*currentWindow.ratio)
            {
                Destroy(floatingTextsGO[i]);
                floatingTextsGO.RemoveAt(i);
            }
        }
    }

    public void setWorldPos()
    {
     worldPos = new Vector3(transform.position.x + 1 * currentWindow.ratio, transform.position.y, transform.position.z);
    }

    // Destroy all of the floating texts
    public void clearFloatingTexts()
    {
        foreach(GameObject floatingText in floatingTextsGO)
        {
            if (floatingText != null)
            {
                Destroy(floatingText);
            }
        }
        floatingTextsGO.Clear();
    }

    // Get xp and possibly level up
    public void GetXP(int XPToGet)
    {
        XP += XPToGet;
        while (XP >= XPTreshold)
        {
            LevelUp();
            refreshHealth();
            recalculations();
            XPTreshold *= 2;
        }
    }

    // LEvel up hero
    public virtual void LevelUp()
    {
        Level++;
    }

    // Refresh the health amount
    public void refreshHealth()
    {
        maxLife = endurance * 10;
        if (maxLife - life > 10)
        {
            life += 10;
        }
        else
        {
            life += maxLife - life;
        }
    }

    public virtual void recalculations()
    {
        recalculateDMG();
        updateSpeeds();
    }
    // Recalculate the damge
    public virtual void recalculateDMG()
    {
        switch (mainAttribiute)
        {
            case MainAtribiute.Strength:
                finalDMG = strength;
                break;
            case MainAtribiute.Agility:
                finalDMG = agility;
                break;
            case MainAtribiute.Inteligence:
                finalDMG = inteligence;
                break;
            case MainAtribiute.Wisdom:
                finalDMG = wisdom;
                break;
            case MainAtribiute.Charisma:
                finalDMG = charisma;
                break;
            case MainAtribiute.Speed:
                finalDMG = speed;
                break;
        }


    }

    // Get the enemy entity and add it to the list
    public void setEnemies(List<EntityBase> enemies)
    {
        enemiesList.Clear();
        foreach(EntityBase enemy in enemies)
        {
            if (enemy != null)
            {
                enemiesList.Add(enemy);
            }
        }
        
    }

    // Clears the enemiesList
    public void clearList()
    {
        enemiesList.Clear();
    }

    // Checks if there is any enemy left alive
    public bool isThereEnemy()
    {
        bool result = false;
        if (enemiesList.Count > 0)
        {
            for (int index = 0; index < enemiesList.Count; index++)
            {
                if (enemiesList[index] != null)
                {
                    if (enemiesList[index].life > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        enemiesList.RemoveAt(index);
                    }
                }
                else
                {
                    enemiesList.RemoveAt(index);
                }
            }

        }
        if (enemiesList.Count <= 0)
        {
            result = false;
        }
        return result;
    }

    // Destroy this game object
    public virtual void destroyMe()
    {
        clearFloatingTexts();
        Destroy(this.gameObject);
    }

    // initialize script that replaces constructior and allows to instantiate this object with different start stats
    // If the initialized stats are equal to 0, than the default stats are being used
    public virtual void initialize( int _strength =0, int _agility =0, int _speed =0, int _endurance =0, int _charisma =0, int _inteligance =0, int _wisdom =0)
    {

        if (_strength != 0)
            strength = _strength;

        if (_agility != 0)
            agility = _agility;

        if (_speed != 0)
            speed = _speed;

        if (_endurance != 0)
            endurance = _endurance;

        if (_charisma != 0)
            charisma = _charisma;

        if (_inteligance != 0)
            inteligence = _inteligance;

        if (_wisdom != 0)
            wisdom = _wisdom;

        life = endurance * 10;
        maxLife = life;
    }

    // Draws floating text over the character
    public void drawFloatingText(string textToDisplay, Color colorToUse)
    {
        textGameObject = new GameObject();
        textGameObject.name = "Floating Text";
        floatingText = textGameObject.AddComponent<TextMesh>();
        if (!drawGUI)
        {
            textGameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        floatingText.text = textToDisplay;
        floatingText.font = Resources.Load<Font>("Font/Arial/ARIAL");
        floatingText.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/3d Text Material");
        floatingText.GetComponent<MeshRenderer>().material.color = colorToUse;
        floatingText.fontSize = 120;
        textGameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        textGameObject.transform.position = new Vector3 (transform.position.x + currentWindow.ratio/2 - textToDisplay.Length*0.2f, transform.position.y + 1 * 1.5f * currentWindow.ratio,transform.position.z) ;

        floatingTextsGO.Add(textGameObject);
    }

    // Deals damage to the enemy
    public virtual void dealDamageToEnemy()
    {
        foreach (EntityBase target in targetEnemy)
        {
            if (target != null)
            {
                print(target.name);
                target.takeDamageFromEnemy(DamageToDeal);
            }
        }
        targetEnemy.Clear();
    }

    // Takes damage from the enemy
    public virtual void takeDamageFromEnemy(int Dmg)
    {
        int x;
        x = Random.Range(1, 101);
        dodgeChance = (agility >= Dmg) ? 16 : 16 * agility / Dmg;
        dodgeChance += dodgeBuff;
        if (dodgeChance >= x)
        {
            drawFloatingText("Dodge!", Color.green);
        }
        else
        {
            drawFloatingText(Dmg.ToString(), Color.red);
            life -= Dmg;
        }
    }

    // Standard Fight script, causes the hero to attack the first enemy in line
    public virtual void fightEnemies()
    {
        if (enemiesList.Count > 0)
        {
            if (isThereEnemy())
            {
                if (isThereAnySkillAvailable())
                {
                    targetingSkill.useSkill(this);
                    useBestAvailableSkill();
                }
            }
        }
    }

    bool isThereAnySkillAvailable()
    {
        if (myOffenseSkills.Count > 0)
        {
            foreach (SOffensive skill in myOffenseSkills)
            {
                if (skill != null)
                {
                    if (Time.time - skill.SkillLastUsed > skill.SkillCooldown)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void useBestAvailableSkill()
    {
        List<SOffensive> tempList = new List<SOffensive>();
        SkillComparer cpr = new SkillComparer();
        for (int s = 0; s< myOffenseSkills.Count; s++)
        {
            if (Time.time - myOffenseSkills[s].SkillLastUsed > myOffenseSkills[s].SkillCooldown && myOffenseSkills[s].skillLevel > 0)
            {
                tempList.Add(myOffenseSkills[s]);
            }
        }
        tempList.Sort(cpr);
        foreach (SOffensive skill in tempList)
        {
            print(skill);
        }
        tempList[0].useSkill(this);
    }
}
