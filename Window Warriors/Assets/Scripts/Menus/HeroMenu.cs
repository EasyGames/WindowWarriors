using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class HeroMenu: WindowBase{

    public List<string> HeroNames = new List<string>();
    public List<GameObject> HeroPrefabs = new List<GameObject>();
    public WindowBase dock;

    public GameObject plusMark;
    GameObject visiblePlus;


    private List<GameObject> HerosList = new List<GameObject>();
    public List<GameObject> HerosShadowList = new List<GameObject>();
    Dictionary<string, GameObject> heroPrefabList = new Dictionary<string, GameObject>();

    float windowWidth;
    Vector3 screenEndPos;

    public GameObject panel;
    RectTransform panelRect;
    public GameObject panelText;

    public GameObject[] heroCharWindowItems;

    public Scrollbar scrollbar;


    // Use this for initialization
    public override void Start()
    {
        panel.SetActive(false);
        screenEndPos = new Vector3(Screen.width,0,30);
        screenEndPos = Camera.main.ScreenToWorldPoint(screenEndPos);

        panelRect = panel.GetComponent<RectTransform>();

        position = new Vector3(0,0,30);
        position = Camera.main.ScreenToWorldPoint(position);

        windowWidth = screenEndPos.x - position.x;
        transform.localScale = new Vector3(windowWidth, transform.localScale.y, transform.localScale.z);
        transform.parent.position = position;
        base.Start();
        transform.position = marker.transform.position + Vector3.right * windowWidth/2 + Vector3.up * 2.5f;
        currentState = windowState.maximized;
        maximizedSize = new Vector3(windowWidth,5,0);
        for (int i = 0; i < HeroNames.Count; i++)
        {
            heroPrefabList.Add(HeroNames[i], HeroPrefabs[i]);
        }
        panelRect.position = transform.position + Vector3.up * transform.localScale.y*3f - Vector3.right*9;
        unlockHero("Dannyl", "warrior");
        unlockHero("Rouge", "rouge");

    }

    public override void minimizeWindow()
    {
        targetPos = Camera.main.WorldToScreenPoint(transform.position);
        if (doOnlyOnce)
        {
            doOnlyOnce = false;
            GetComponent<BoxCollider>().enabled = false;
            foreach (EntityBase hero in herosList)
            {
                hero.transform.parent.gameObject.GetComponent<BoxCollider>().enabled = false;
                hero.transform.GetComponent<SpriteRenderer>().enabled = false;
                hero.drawGUI = false;
            }
        }
        transform.localScale = Vector3.Lerp(transform.localScale, minimizedSize, Time.deltaTime * minimaximazingSpeed);
        transform.position = Vector3.Lerp(transform.position, marker.transform.position + Vector3.right * 0.5f + Vector3.up * 0.5f, Time.deltaTime * minimaximazingSpeed);
        doOnlyOnce = false;



        if (transform.localScale == minimizedSize)
        {
            marker.ChangeSize = false;
            thisRenderer.enabled = false;
            doOnlyOnce = true;
            currentState = windowState.minimized;
        }
        foreach (GameObject shadow in HerosShadowList)
        {
            shadow.GetComponent<SpriteRenderer>().enabled = false;
            shadow.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public override void maximizeWindow()
    {
        targetPos = Camera.main.WorldToScreenPoint(transform.position);
        if (doOnlyOnce)
        {
            GetComponent<BoxCollider>().enabled = true;
            if (visiblePlus != null)
            {
                Destroy(visiblePlus);
            }
            thisRenderer.enabled = true;
            doOnlyOnce = false;
        }
        transform.localScale = Vector3.Lerp(transform.localScale, maximizedSize, Time.deltaTime * minimaximazingSpeed);
        transform.position = Vector3.Lerp(transform.position, marker.transform.position + Vector3.up * 2.5f + Vector3.right * windowWidth/2, Time.deltaTime * minimaximazingSpeed);
        if (transform.localScale == maximizedSize)
        {
            foreach (EntityBase hero in herosList)
            {
                hero.transform.parent.gameObject.GetComponent<BoxCollider>().enabled = true;
                hero.transform.GetComponent<SpriteRenderer>().enabled = true;
                hero.drawGUI = true;
            }
            foreach (GameObject shadow in HerosShadowList)
            {
                shadow.GetComponent<SpriteRenderer>().enabled = true;
                shadow.GetComponent<BoxCollider>().enabled = true;
            }
            currentState = windowState.maximized;
            marker.ChangeSize = false;
            doOnlyOnce = true;
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            HerosShadowList[0].transform.position = transform.position;
        }
	
	}

    public void unlockHero(string editorName, string className)
    {
        GameObject instantiatedHero;
        GameObject shadow;
        DragScript enityDragScript;
        instantiatedHero = (GameObject)Instantiate(heroPrefabList[className], new Vector3(0, 0, 0), Quaternion.identity);
        instantiatedHero.name = editorName;
        enityDragScript = instantiatedHero.GetComponent<DragScript>();
        enityDragScript.mainDock = dock;
        shadow = createShadow(instantiatedHero.transform.GetChild(0).GetComponent<EntityBase>());
        instantiatedHero.GetComponent<DragScript>().myShadow = shadow.GetComponent<ShadowScript>();
        HerosList.Add(instantiatedHero);

    }


    // function to add hero to window hoeroslist
    public override void addHero(EntityBase hero)
    {
        if (hero != null && !herosList.Contains(hero))
        {
            herosList.Add(hero);
            if (currentState == windowState.minimized)
            {
                hero.GetComponent<SpriteRenderer>().enabled = false;
                hero.transform.parent.GetComponent<BoxCollider>().enabled = false;
                hero.drawGUI = false;
                visiblePlus = (GameObject)Instantiate(plusMark, marker.transform.position + Vector3.right, Quaternion.identity);
            }
        }

        for (int i = 0; i < HerosShadowList.Count; i++)
        {
            HerosShadowList[i].transform.position = position + Vector3.right + Vector3.right * i* 1.2f + Vector3.up * 2.0f;
        }

        ShadowScript shadow = hero.transform.parent.GetComponent<DragScript>().myShadow;
        hero.transform.parent.position = shadow.transform.position;
        shadow.GetComponent<SpriteRenderer>().enabled = false;
        shadow.GetComponent<BoxCollider>().enabled = false;

        /*
        foreach (GameObject shadow in HerosShadowList)
        {
            if (shadow.GetComponent<ShadowScript>().shadowedScript == hero)
            {
                hero.transform.parent.position = shadow.transform.position;
                shadow.GetComponent<SpriteRenderer>().enabled = false;
                shadow.GetComponent<BoxCollider>().enabled = false;
                break;
            }
        }
        */

        sendEnemies();
    }

    // function to remove hero from windows herolist
    public override void removeHero(EntityBase hero)
    {
        if (hero != null)
        {
            herosList.Remove(hero);
        }
        foreach (GameObject shadow in HerosShadowList)
        {
            if (shadow.GetComponent<ShadowScript>().shadowedScript == hero)
            {
                shadow.GetComponent<SpriteRenderer>().enabled = true;
                shadow.GetComponent<BoxCollider>().enabled = true;
                break;
            }
        }
        sendEnemies();
    }

    GameObject createShadow(EntityBase hero)
    {
        print("creating shadow");
        GameObject tempGO = new GameObject();
        tempGO.name = hero.transform.parent.name;
        tempGO.AddComponent<SpriteRenderer>().sprite = hero.GetComponent<SpriteRenderer>().sprite;
        tempGO.GetComponent<SpriteRenderer>().color = Color.gray;
        tempGO.AddComponent<ShadowScript>().shadowedScript = hero;
        tempGO.AddComponent<BoxCollider>().center = new Vector3(0.5f, 0.5f, 0);
        tempGO.GetComponent<ShadowScript>().heroMenu = this;
        tempGO.GetComponent<ShadowScript>().panel = panel;
        tempGO.GetComponent<ShadowScript>().panelText = panelText;
        tempGO.GetComponent<ShadowScript>().heroCharWindowItems = heroCharWindowItems;
        tempGO.GetComponent<ShadowScript>().scrollbar = scrollbar;
        tempGO.layer = 8;
        HerosShadowList.Add(tempGO);
        return tempGO;
    }

    public GameObject currentlySlecetedShadow()
    {
        foreach (GameObject shadow in HerosShadowList)
        {
            if (shadow.GetComponent<ShadowScript>().displayStats)
            {
                return shadow;
            }
        }
        return null;
    }
}
