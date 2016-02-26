using UnityEngine;
using System.Collections;

public class DragScript : MonoBehaviour {

    private bool dragging = false;
    private float distance;

    public ShadowScript myShadow;
    public WindowBase mainDock;

    RaycastHit hit;
    LayerMask heroLayer = 1 << 8;
    LayerMask ignoreHeroLayer;
    Vector3 originalPosition;
    Vector3 dockPosition;

    // monster manager with script
    GameObject previousWindow;
    WindowBase previousWidowBaseScript;
    public WindowBase currentWindowBaseScript;

    // Hero and his script
    GameObject hero;
    EntityBase heroScript;

    // Statistic window display
    bool displayStats = false;

    // Child sprite renderer
    SpriteRenderer childRender;
    SpriteRenderer rendererClone;

    // Float for telling apart drag from click.
    float timeToDrag;
    bool checkForDrag = true; 




    void Start()
    {
        distance = Vector3.Distance(Camera.main.transform.position, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0));
        ignoreHeroLayer = ~heroLayer;
        hero = transform.GetChild(0).gameObject;
        if (hero != null)
        {
            heroScript = hero.GetComponent<EntityBase>();
        }
        mainDock.addHero(heroScript);
        previousWidowBaseScript = mainDock;
        currentWindowBaseScript = mainDock;

        //Rendering the same sprite
        childRender = transform.GetChild(0).GetComponent<SpriteRenderer>();

        myShadow.equipmentChanged += recalcualteDMG;
    }

    void OnMouseEnter()
    {
        childRender.color = new Color(0.5f, 1.0f, 0.5f);
        myShadow.GetComponent<SpriteRenderer>().color = new Color(0.3f, 1.0f, 0.3f);

    }


    void OnMouseExit()
    {
        childRender.color = Color.white;
        myShadow.GetComponent<SpriteRenderer>().color = Color.gray;
        displayStats = false;
    }

    void OnMouseDown()
    {
        timeToDrag = Time.time;
        originalPosition = transform.position;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, ignoreHeroLayer);
        if (hit.collider.tag == "Window")
        {
            if (heroScript.arrowDelegate != null)
            {
                heroScript.arrowDelegate();
            }
            previousWindow = hit.collider.gameObject;
            previousWidowBaseScript = previousWindow.GetComponent<WindowBase>();
        }

    }

    void OnMouseDrag()
    {
        if (Time.time - timeToDrag >= 0.18f && checkForDrag)
        {
            checkForDrag = false;
            dragging = true;
            previousWidowBaseScript.removeHero(heroScript);
            heroScript.clearList();
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1))
        {
            currentWindowBaseScript.removeHero(heroScript);
            previousWidowBaseScript = currentWindowBaseScript;
            currentWindowBaseScript = mainDock;
        

            if (previousWidowBaseScript.herosList.Count <= 0)
            {
                previousWidowBaseScript.doOnce = true;
                previousWidowBaseScript.currentWave = 0;
            }
            else
            {
                previousWidowBaseScript.currentWave = 1;
            }
            currentWindowBaseScript.addHero(heroScript);
            heroScript.setEnemies(currentWindowBaseScript.getEnemies());
        }
    }

    void OnMouseUp()
    {
        if(checkForDrag)
        {
            myShadow.ShowStatWindow();
            dragging = false;
            checkForDrag = true;
        }
        else
        {
            checkForDrag = true;
            dragging = false;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, ignoreHeroLayer);
            if (hit.collider.tag == "Window")
            {
                currentWindowBaseScript = hit.collider.GetComponent<WindowBase>();
                if (currentWindowBaseScript.maxNumberOfHeros == currentWindowBaseScript.herosList.Count)
                {
                    previousWidowBaseScript.addHero(heroScript);
                }
                else
                {
                    if (currentWindowBaseScript == previousWidowBaseScript)
                    {

                    }
                    else
                    {
                        if (previousWidowBaseScript.herosList.Count <= 0)
                        {
                            previousWidowBaseScript.doOnce = true;
                            previousWidowBaseScript.currentWave = 0;
                        }
                        else
                        {
                            previousWidowBaseScript.currentWave = 1;
                        }
                    }
                    transform.position = hit.collider.transform.position - Vector3.right * 3 - Vector3.up * 0.8f;
                    currentWindowBaseScript.addHero(heroScript);
                    heroScript.setEnemies(currentWindowBaseScript.getEnemies());
                }

            }
            else
            {
                previousWidowBaseScript.addHero(heroScript);
            }
        }

    }
    
    public virtual void Update()
    {
        if (dragging)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance-10);
            transform.position = Camera.main.ScreenToWorldPoint(position) - Vector3.up *0.5f - Vector3.right*0.5f;
        }
    }

    public void OnGUI()
    {

        if (displayStats)
        {
            string stats = "Name: " + transform.name +
            "\nLevel: " + transform.GetChild(0).GetComponent<EntityBase>().Level +
            "\nXP: " + transform.GetChild(0).GetComponent<EntityBase>().XP +
            "\nLife: " + transform.GetChild(0).GetComponent<EntityBase>().life +
            "\nMaxLife: " + transform.GetChild(0).GetComponent<EntityBase>().maxLife +
            "\nStrength: " + transform.GetChild(0).GetComponent<EntityBase>().strength +
            "\nAgility: " + transform.GetChild(0).GetComponent<EntityBase>().agility +
            "\nSpeed: " + transform.GetChild(0).GetComponent<EntityBase>().speed +
            "\nEndurance: " + transform.GetChild(0).GetComponent<EntityBase>().endurance +
            "\nInteligence: " + transform.GetChild(0).GetComponent<EntityBase>().inteligence +
            "\nWisdom: " + transform.GetChild(0).GetComponent<EntityBase>().wisdom +
            "\nCharisma: " + transform.GetChild(0).GetComponent<EntityBase>().charisma;
            Vector2 positionGUI = Camera.main.WorldToScreenPoint(transform.position);
            GUI.Box(new Rect(positionGUI.x, Screen.height- positionGUI.y, 150, 200), stats);
        }

    }

    public void recalcualteDMG()
    {
        heroScript.recalculateDMG();
    }
}
