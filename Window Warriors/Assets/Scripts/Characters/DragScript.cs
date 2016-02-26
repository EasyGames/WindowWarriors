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

    // hero position in window
    enum heroPosition { position1, position2, position3, none };
    int indexToAddHero;



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
                    if (indexToAddHero == -1)
                    {
                        previousWidowBaseScript.addHero(heroScript);
                    }
                    else
                    {
                        EntityBase previousHero = currentWindowBaseScript.herosList[indexToAddHero];
                        previousHero.transform.parent.GetComponent<DragScript>().currentWindowBaseScript = mainDock;
                        previousHero.transform.parent.GetComponent<DragScript>().previousWidowBaseScript = currentWindowBaseScript;
                        currentWindowBaseScript.herosList[indexToAddHero] = heroScript;
                        currentWindowBaseScript.refreshHeroPositions();
                        mainDock.addHero(previousHero);
                        print("index to add" + indexToAddHero);
                    }
                }
                else
                {

                    if (indexToAddHero != -1)
                    {
                        switch (indexToAddHero)
                        {
                            case 2:
                                break;
                            case 1:
                                if (currentWindowBaseScript.herosList.Count > 1)
                                {
                                    if (currentWindowBaseScript.herosList[1] != null)
                                    {
                                        EntityBase middlePositionHero = currentWindowBaseScript.herosList[1];
                                        currentWindowBaseScript.herosList.Insert(2, middlePositionHero);
                                    }
                                    currentWindowBaseScript.herosList[1] = heroScript;
                                }
                                else
                                {
                                    currentWindowBaseScript.herosList.Insert(1, heroScript);
                                }
                            
                                break;
                            case 0:
                                if (currentWindowBaseScript.herosList.Count > 1)
                                {
                                    if (currentWindowBaseScript.herosList[1] != null)
                                    {
                                        EntityBase middlePositionHero = currentWindowBaseScript.herosList[1];
                                        currentWindowBaseScript.herosList.Insert(2, middlePositionHero);

                                        EntityBase firstPositionHero = currentWindowBaseScript.herosList[0];
                                        currentWindowBaseScript.herosList[1] = firstPositionHero;
                                    }
                                    currentWindowBaseScript.herosList[0] = heroScript;
                                }
                                else if (currentWindowBaseScript.herosList.Count > 0)
                                {
                                    if (currentWindowBaseScript.herosList[0] != null)
                                    {
                                        EntityBase firstPositionHero = currentWindowBaseScript.herosList[0];
                                        currentWindowBaseScript.herosList.Insert(1,firstPositionHero);
                                    }
                                    currentWindowBaseScript.herosList[0] = heroScript;
                                }
                                else
                                {
                                    currentWindowBaseScript.herosList.Insert(0, heroScript);
                                }
                                break;
                        }
                        currentWindowBaseScript.refreshHeroPositions();
                    }
                    if (currentWindowBaseScript == previousWidowBaseScript)
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
            heroPosition currentPosition = heroPosition.none;
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance-10);
            transform.position = Camera.main.ScreenToWorldPoint(position) - Vector3.up *0.5f - Vector3.right*0.5f;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100,ignoreHeroLayer))
            {
                if (hit.collider.tag == "Window")
                {
                    Vector3 positionDifference = hit.collider.transform.position - hit.point;

                    if (positionDifference.x >= 4.4f && currentPosition != heroPosition.position3)
                    {
                        currentPosition = heroPosition.position3;
                        indexToAddHero = 2;
                    }
                    else if(positionDifference.x >= 3.2f && positionDifference.x < 4.4f && currentPosition != heroPosition.position3)
                    {
                        currentPosition = heroPosition.position2;
                        indexToAddHero = 1;
                    }
                    else if(positionDifference.x < 3.2f && currentPosition != heroPosition.position3)
                    {
                        currentPosition = heroPosition.position1;
                        indexToAddHero = 0;
                    }
                }
                else
                {
                    currentPosition = heroPosition.none;
                    indexToAddHero = -1;
                }
            }
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
