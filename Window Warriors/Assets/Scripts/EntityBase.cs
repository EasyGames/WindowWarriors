using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//Hero class
public enum heroClass { All, Warrior, Archer, Mage  };

public class EntityBase : MonoBehaviour {

    //Hero class
    public heroClass myClass;
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
    public Color orange { get; set; }
    public Vector3 targetPos { get; set; }
    public Vector3 worldPos { get; set; }
    public int lifeHeight { get; set; }
    public int lifeWidth { get; set; }
    public bool drawGUI { get; set; }

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
    public int Endurance= 10;
    public int Charisma = 10;
    public int Inteligence = 10;
    public int Wisdom = 10;
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
        worldPos = transform.position + Vector3.right;
        life = Endurance * 10;
        maxLife = life;
        XPTreshold = (Level == 1) ? 100 : (100 * (2 ^ (Level - 1)));
        finalDMG = strength;
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
            if (floatingTextsGO[i].transform.position.y >= transform.position.y + 2.0f)
            {
                Destroy(floatingTextsGO[i]);
                floatingTextsGO.RemoveAt(i);
            }
        }
    }

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

    public void GetXP(int XPToGet)
    {
        XP += XPToGet;
        if (XP >= XPTreshold)
        {
            LevelUp();
            XPTreshold *= 2;
        }
    }

    public virtual void LevelUp()
    {
        Level++;
        refreshHealth();
    }

    void refreshHealth()
    {
        maxLife = Endurance * 10;
        if (maxLife - life > 10)
        {
            life += 10;
        }
        else
        {
            life += maxLife - life;
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
        Destroy(textGameObject);
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
            Endurance = _endurance;

        if (_charisma != 0)
            Charisma = _charisma;

        if (_inteligance != 0)
            Inteligence = _inteligance;

        if (_wisdom != 0)
            Wisdom = _wisdom;

        life = Endurance * 10;
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
                GUIDrawRect(new Rect(targetPos.x, Screen.height - targetPos.y, lifeWidth, -life / maxLife * lifeHeight), Color.green);
            }
            else if (life > maxLife / 5)
            {
                GUIDrawRect(new Rect(targetPos.x, Screen.height - targetPos.y, lifeWidth, -life / maxLife * lifeHeight), orange);
            }
            else
            {
                GUIDrawRect(new Rect(targetPos.x, Screen.height - targetPos.y, lifeWidth, -life / maxLife * lifeHeight), Color.red);
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
        floatingText.color = colorToUse;
        if (!drawGUI)
        {
            textGameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        floatingText.text = textToDisplay;
        floatingText.fontSize = 120;
        textGameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        textGameObject.transform.position = transform.position + Vector3.up * 1.5f;

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
