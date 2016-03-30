using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindowBase : MonoBehaviour {

    // Window event variables
    public GameObject BlockBackground;
    public float ratio = 1;
    public int addWaves { get; set; }
    public int currentWave { get; set; }
    public int wavesToBeFinished;
    public Vector3 targetPos { get; set; }
    Vector2 rectSize = new Vector2(50,20);
    float WindowHeight;
    float peviousTime;
    public bool windowCleared { get; set; }
    public bool isHeroResting { get; set; }
    public bool doOnce { get; set; }
    public GameObject reward;
    public int rewardAmount;
    public HeroMenu heroMenu;
    public bool showTimer { get; set; } 
    public float raidTime { get; set; }
    public float lastRaidTime { get; set; }

    // Money manager Script
    public MoneyManager moneyManager;

    // Window type
    public enum windowType { FightingLooting, Static, Resourecs }
    public windowType thisWindowType;

    // minimizing and maximizing widow
    public float minimaximazingSpeed = 20.0f;
    public Vector3 minimizedSize = new Vector3(1.2f,0.2f,0.1f);
    public Vector3 maximizedSize = new Vector3(12.0f, 2.0f, 1.0f);
    Vector3 heroMinimized = new Vector3(0.1f, 0.1f, 0.1f);
    Vector3 heroMaximized = new Vector3(1.0f, 1.0f, 1.0f);
    public enum windowState {minimized, maximized, fullScreen};
    public windowState currentState { get; set; }
    public MeshRenderer thisRenderer { get; set; }
    public PlaceMarker marker;
    public bool doOnlyOnce { get; set; }
    
    // Background of this window and it's scrolling
    public Sprite background;
    public float scrollingSpeed = 0.1f;
    Renderer rend;
    float offset;

    // Heros list
    public List<EntityBase> herosList {get; set;}
    public int maxNumberOfHeros = 3;

    // Enemies list and objecs
    public Vector3 position { get; set;}
    public EntityBase enemy { get; set;}
    public List<EntityBase> enemiesList { get; set;}

    public virtual void Start () {
        addWaves = 2;
        currentWave = 0;
        doOnce = true;
        showTimer = false;
        doOnlyOnce = true;

        // Posotion to display GUI
        WindowHeight = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, transform.localScale.y/2, 0)).y - Camera.main.WorldToScreenPoint(transform.position).y;
        targetPos = Camera.main.WorldToScreenPoint(transform.position);

        // Setting event variables
        isHeroResting = false;

        transform.position = marker.transform.position + Vector3.up * 2.0f + Vector3.right * 0.5f;
        // Get this object mesh renderer
        thisRenderer = GetComponent<MeshRenderer>();

        // initiaslize heros list
        herosList = new List<EntityBase>();

        // initialize enemies list;
        enemiesList = new List<EntityBase>();

        // initialize render component
        rend = GetComponent <Renderer>();

        // Getting the current state of the window size
        currentState = (transform.localScale == maximizedSize) ? windowState.maximized : windowState.minimized;

        // changing sprite to texture
        Texture2D croppedTexture = new Texture2D((int)background.rect.width, (int)background.rect.height);
        Color[] pixels = background.texture.GetPixels((int)background.textureRect.x,
                                                (int)background.textureRect.y,
                                                (int)background.textureRect.width,
                                                (int)background.textureRect.height);
        // setting texture properties and apply them
        croppedTexture.SetPixels(pixels);
        croppedTexture.filterMode = FilterMode.Point;
        croppedTexture.wrapMode = TextureWrapMode.Repeat;
        croppedTexture.Apply();
        
        // assigning texture top the material
        this.GetComponent<Renderer>().material.mainTexture = croppedTexture;
    }

    // function to add hero to window hoeroslist
    public virtual void addHero(EntityBase hero)
    {
        if (hero != null && !herosList.Contains(hero))
        {
            herosList.Add(hero);
        }
        refreshHeroPositions();
        sendEnemies();
    }

    // function to remove hero from windows herolist
    public virtual void removeHero(EntityBase hero)
    {
        if (hero != null)
        {
            herosList.Remove(hero);
        }
        refreshHeroPositions();
        sendEnemies();
    }

    // Refresh the positions on which heros are located;
    public void refreshHeroPositions()
    {
        for (int i = 0; i < herosList.Count; i++)
        {
            if (herosList[i] != null)
            {
                herosList[i].gameObject.transform.parent.position = transform.position - Vector3.right * i * 1.2f*ratio - Vector3.right * 3*ratio - Vector3.up * 0.8f*ratio;
            }
        }
        sendEnemies();
    }

    public virtual void refreshEnemiesPositions()
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemiesList[i] != null)
            {
                enemiesList[i].gameObject.transform.position = transform.position + Vector3.right * i * 1.2f * ratio + Vector3.right * 2 * ratio - Vector3.up * 0.8f * ratio;
                enemiesList[i].GetComponent<EntityBase>().setWorldPos();
            }
        }
    }

    public virtual void refreshRewardPosition()
    {
        if (this.reward != null)
        {
            EntityBase rewardScript = reward.GetComponent<EntityBase>();
            rewardScript.transform.position = (ratio > 1) ? transform.position - (Vector3.up*(transform.localScale.y/2))  : transform.parent.position + Vector3.up;
            rewardScript.currentWindow = this;
            rewardScript.setWorldPos();
        }
    }

    // send the enemies from the enemies list to every hero on herosList and vice versa
    public void sendEnemies()
    {
        foreach (HeroBase hero in herosList)
        {
            hero.setEnemies(enemiesList);
        }

        foreach (EntityBase monster in enemiesList)
        {
            
            monster.setEnemies(herosList);
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (currentState == windowState.maximized)
            {
                fullscreenMode();
                currentState = windowState.fullScreen;
            }
            else if (currentState == windowState.fullScreen)
            {
                fullscreenModeExit();
                currentState = windowState.maximized;
            }
        }
    }

    // A function to check if there is any enemy left alive
    public bool isThereEnemy()
        {
            while (enemiesList.Count > 0)
            {
            if (enemiesList[0] != null)
            {
                if (enemiesList[0].GetComponent<EntityBase>().life > 0)
                    return true;
                else
                    enemiesList.RemoveAt(0);
            }
            else
                enemiesList.RemoveAt(0);
        }
            return false;
        }

    public bool isThereHeroAlive()
    {
        foreach(EntityBase hero in herosList)
        {
            if (hero != null)
            {
                if (hero.life > 0)
                    return true;
            }
        }
        return false;
    }

    public bool isThereHeroWalking()
    {
        foreach (EntityBase hero in herosList)
        {
            if (hero != null)
            {
                if (hero.walking)
                    return true;
            }
        }
        return false;
    }

    public bool isHerosMaxHealth()
    {
        int countHeros = 0;
        foreach (EntityBase hero in herosList)
        {
            if (hero != null)
            {
                if(hero.life == hero.maxLife)
                {
                    countHeros++;
                }
            }
        }
        if (countHeros == herosList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // A call to get enemies from this window to be used in other objects.
    public List<EntityBase> getEnemies()
    {
        return enemiesList;
    }

    // Update is called once per frame
    public virtual void FixedUpdate ()
    {
        if (herosList.Count > 0)
        {
            if (isThereHeroWalking() && !isHeroResting)
            {
                offset = Time.deltaTime * scrollingSpeed;
                rend.material.mainTextureOffset += new Vector2(offset, 0);
                if (rend.material.mainTextureOffset.x >= 1.0f)
                {
                    rend.material.mainTextureOffset = new Vector2(0.0f, 0);
                }
            }
        }

        if (marker.ChangeSize == true)
        {
            if(currentState == windowState.maximized)
            {
                minimizeWindow();
            }
            else
            {
                maximizeWindow();
            }

        }

        if (!isThereHeroAlive())
        {
            if (isThereEnemy())
            {
                foreach(EntityBase enemy in enemiesList)
                {
                    enemy.clearFloatingTexts();
                    Destroy(enemy.gameObject);
                }
                enemiesList.Clear();
                if(herosList.Count <= 0)
                {
                    currentWave = 0;
                }
                else
                {
                    currentWave -= 1;
                }
            }
            if ( herosList.Count > 0)
                isHeroResting = true;
        }

        if (isHeroResting)
        {
            if (Time.time - peviousTime >= 2.0f)
            {
                foreach (EntityBase hero in herosList)
                {
                    if (hero != null)
                    {
                        hero.life += 5;
                        if (hero.life >= hero.maxLife)
                        {
                            hero.life = hero.maxLife;
                        }
                    }
                }
                peviousTime = Time.time;
            }

            if (isHerosMaxHealth())
            {
                isHeroResting = false;
            }
        }


        if (currentState == windowState.minimized || marker.ChangeSize)
        {
            foreach (EntityBase enemy in enemiesList )
            {
                if (enemy != null)
                {
                enemy.GetComponent<SpriteRenderer>().enabled = false;
                enemy.drawGUI = false;
                }
            }
        }
        else if (currentState == windowState.maximized)
        {

            foreach (EntityBase enemy in enemiesList)
            {
                if (enemy != null)
                {
                    enemy.GetComponent<SpriteRenderer>().enabled = true;
                    enemy.drawGUI = true;
                }
            }
        }
    }

    public virtual void fullscreenMode()
    {
        float leftSiteOftheScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)).x;
        float rightSiteOfTheScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, -Camera.main.transform.position.z)).x;
        Vector3 middleOfTheScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, -Camera.main.transform.position.z-1));
        float fullScreenSize = rightSiteOfTheScreen - leftSiteOftheScreen;
        ratio = fullScreenSize / transform.localScale.x;
        transform.localScale = new Vector3(fullScreenSize, transform.localScale.y * ratio, 1);
        transform.position = middleOfTheScreen;
        BlockBackground.SetActive(true);
        for (int i = 0; i < herosList.Count; i++)
        {
            if (herosList[i] != null)
            {
                Vector3 heroLocalScale = herosList[i].gameObject.transform.parent.localScale;
                herosList[i].gameObject.transform.parent.localScale = new Vector3(heroLocalScale.x * ratio, heroLocalScale.y * ratio, heroLocalScale.z);
            }
        }
        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemiesList[i] != null)
            {
                print("Enemy: " + enemiesList[i].name);
                Vector3 enemyLocalScale = enemiesList[i].gameObject.transform.localScale;
                enemiesList[i].gameObject.transform.localScale = new Vector3(enemyLocalScale.x * ratio, enemyLocalScale.y * ratio, enemyLocalScale.z);
            }
        }
        if (reward != null)
        {
            Vector3 rewardLocalScale = reward.transform.localScale;
            reward.transform.localScale = new Vector3(rewardLocalScale.x * ratio, rewardLocalScale.y * ratio, rewardLocalScale.z);
        }
        refreshHeroPositions();
        refreshEnemiesPositions();
        refreshRewardPosition();
    }

    public virtual void fullscreenModeExit()
    {
        for (int i = 0; i < herosList.Count; i++)
        {
            if (herosList[i] != null)
            {
                Vector3 heroLocalScale = herosList[i].gameObject.transform.parent.localScale;
                herosList[i].gameObject.transform.parent.localScale = new Vector3(heroLocalScale.x / ratio, heroLocalScale.y / ratio, heroLocalScale.z);
            }
        }
        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemiesList[i] != null)
            {
                Vector3 enemyLocalScale = enemiesList[i].gameObject.transform.localScale;
                enemiesList[i].gameObject.transform.localScale = new Vector3(enemyLocalScale.x / ratio, enemyLocalScale.y / ratio, enemyLocalScale.z);
            }
        }
        if (reward != null)
        {
            Vector3 rewardLocalScale = reward.transform.localScale;
            reward.transform.localScale = new Vector3(rewardLocalScale.x / ratio, rewardLocalScale.y / ratio, rewardLocalScale.z);
        }
        transform.localScale = maximizedSize;
        transform.position = marker.transform.position + Vector3.up * 2.0f + Vector3.right * 0.5f;
        BlockBackground.SetActive(false);
        ratio = 1;
        refreshHeroPositions();
        refreshEnemiesPositions();
        refreshRewardPosition();
    }

    // minimizing the window
    public virtual void minimizeWindow()
    {
        targetPos = Camera.main.WorldToScreenPoint(transform.position);
        if (doOnlyOnce)
        {
            doOnlyOnce = false;
            GetComponent<MeshCollider>().enabled = false;
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
    }

    // maximizing the window
    public virtual void maximizeWindow()
    {
        targetPos = Camera.main.WorldToScreenPoint(transform.position);
        if (doOnlyOnce)
        {
            GetComponent<MeshCollider>().enabled = true;
            thisRenderer.enabled = true;
            doOnlyOnce = false;
        }
        transform.localScale = Vector3.Lerp(transform.localScale, maximizedSize, Time.deltaTime * minimaximazingSpeed);
        transform.position = Vector3.Lerp(transform.position, marker.transform.position + Vector3.up * 2.0f + Vector3.right * 0.5f, Time.deltaTime * minimaximazingSpeed);
        if (transform.localScale == maximizedSize)
        {
            foreach (EntityBase hero in herosList)
            {
                hero.transform.parent.gameObject.GetComponent<BoxCollider>().enabled = true;
                hero.transform.GetComponent<SpriteRenderer>().enabled = true;
                hero.drawGUI = true;
            }
            currentState = windowState.maximized;
            marker.ChangeSize = false;
            doOnlyOnce = true;
        }
    }

    public virtual void spawnEssentials(EntityBase spawnedEntity)
    {
        spawnedEntity.setEnemies(herosList);
        spawnedEntity.transform.localScale = new Vector3(enemy.transform.localScale.x * ratio, enemy.transform.localScale.y * ratio, enemy.transform.localScale.z);
        spawnedEntity.currentWindow = this;
        spawnedEntity.setWorldPos();
        enemiesList.Add(enemy);
    }

    public virtual void spawnReward(GameObject reward)
    {
        EntityBase rewardScript = reward.GetComponent<EntityBase>();
        rewardScript.transform.localScale = new Vector3(rewardScript.transform.localScale.x * ratio, rewardScript.transform.localScale.y * ratio, rewardScript.transform.localScale.z);
        rewardScript.currentWindow = this;
        rewardScript.setWorldPos();
        refreshRewardPosition();
    }

    public virtual void OnGUI()
    {
        if (wavesToBeFinished > 0)
        {
            GUI.Box(new Rect(targetPos.x - (rectSize.x/2), Screen.height - targetPos.y - WindowHeight - 21, 50, 20),currentWave + "/" + wavesToBeFinished);
        }
        if (showTimer)
        {
            GUI.Box(new Rect(targetPos.x - (rectSize.x / 2), Screen.height - targetPos.y - WindowHeight - 21, 50, 20), ((int)raidTime + 1 -((int)Time.time - (int)lastRaidTime)).ToString());
        }
    }
}
