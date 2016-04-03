﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//Hero class
public enum heroClass { All, Warrior, Archer, Mage, Monk, Rogue };

public class EntityBase : MonoBehaviour {

    //Hero class
    public heroClass myClass;
    public enum MainAtribiute {Strength, Agility, Inteligence, Wisdom, Charisma, Speed };
    public MainAtribiute mainAttribiute;

    public int finalDMG { get; set; }

    // Archer arrow delegate
    public delegate void ArrowDelegate();
    public ArrowDelegate arrowDelegate;

    //Tile size
    public int tileSize { get; set; }

    // Animation
    public Animator animator { get; set; }
    public bool walking { get; set; }

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
    public int dodgeChance { get; set; }

    // Floating Text
    public GameObject textGameObject { get; set; }
    TextMesh floatingText { get; set; }
    RectTransform canvasTransform { get; set; }
    List<GameObject> floatingTextsGO { get; set; }

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
        recalculateDMG();
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
            recalculateDMG();
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
        while (enemiesList.Count > 0)
        {
            if (enemiesList[0] != null)
            {
                if (enemiesList[0].life > 0)
                {
                    return true;
                }
                else
                {
                    enemiesList.RemoveAt(0);
                }
            }
            else
            {
                enemiesList.RemoveAt(0);
            }
        }
        return false;
    }

    // Standard Fight script, causes the hero to attack the first enemy in line
    public virtual void fightEnemies()
    {
        if (enemiesList.Count > 0)
        {
            if (isThereEnemy())
            {
                // All game entities must inherit Entity base script
                enemyScript = enemiesList[0];
                if (enemyScript.life > 0)
                {
                    dealDamageToEnemy(finalDMG, enemyScript);
                    if (enemyScript.life <= 0)
                    {
                        enemiesList.RemoveAt(0);
                    }
                }
            }
        }
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

    // Drawing the health bar
    public virtual void OnGUI()
    {
        lifeHeight = Screen.height / 32;
        lifeWidth = lifeHeight / 10;
        targetPos = Camera.main.WorldToScreenPoint(worldPos);
        if (life > 0 && drawGUI)
        {
            if (life > maxLife / 2)
            {
                //GUIDrawRect(new Rect(targetPos.x, Screen.height - targetPos.y, lifeWidth * currentWindow.ratio, -life / maxLife * lifeHeight * currentWindow.ratio), Color.green);
            }
            else if (life > maxLife / 5)
            {
                //GUIDrawRect(new Rect(targetPos.x, Screen.height - targetPos.y, lifeWidth * currentWindow.ratio, -life / maxLife * lifeHeight * currentWindow.ratio), orange);
            }
            else
            {
                //GUIDrawRect(new Rect(targetPos.x, Screen.height - targetPos.y, lifeWidth * currentWindow.ratio, -life / maxLife * lifeHeight * currentWindow.ratio), Color.red);
            }

        }
    }

    // Function to draw any colored rectangle on the UI andd only on the UI
    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;
    public static void GUIDrawRect(Rect position, Color color)
    {
        if (_staticRectTexture == null)
        {
            _staticRectTexture = new Texture2D(1, 1);
        }

        if (_staticRectStyle == null)
        {
            _staticRectStyle = new GUIStyle();
        }

        _staticRectTexture.SetPixel(0, 0, color);
        _staticRectTexture.Apply();

        _staticRectStyle.normal.background = _staticRectTexture;

        GUI.Box(position, GUIContent.none, _staticRectStyle);
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
        textGameObject.transform.position = new Vector3 (transform.position.x + currentWindow.ratio/2, transform.position.y + 1 * 1.5f * currentWindow.ratio,transform.position.z) ;

        floatingTextsGO.Add(textGameObject);
    }

    // Deals damage to the enemy
    public virtual void dealDamageToEnemy(int Dmg, EntityBase _entityScript)
    {
        int x;
        x = Random.Range(1, 101);
        criticalChance = (agility >= enemiesList[0].GetComponent<EntityBase>().agility*2) ? 16 : 8 * agility / enemiesList[0].GetComponent<EntityBase>().agility;
        if (criticalChance >= x)
        {
            Dmg *= 2;
        }
        _entityScript.takeDamageFromEnemy(Dmg);
    }

    // Takes damage from the enemy
    public virtual void takeDamageFromEnemy(int Dmg)
    {
        int x;
        x = Random.Range(1, 101);
        dodgeChance = (agility >= Dmg) ? 16 : 16 * agility / Dmg;
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
}
