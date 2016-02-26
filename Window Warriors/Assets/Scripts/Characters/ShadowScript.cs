using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class ShadowScript : MonoBehaviour {

    public EntityBase shadowedScript;
    public bool displayStats = false;
    public HeroMenu heroMenu;
    Image background;

    public GameObject panel;
    public GameObject panelText;
    string stats;

    // 0 - Hero icon slot
    // 1 - Helmet slot
    // 2 - Armor slot 
    // 3 - shoes slot
    // 4 - right hand slot
    // 5 - left hand slot
    public GameObject[] heroCharWindowItems;
    public GameObject[] currentEquipment;

    public Scrollbar scrollbar;
    


    void Start()
    {
        
        shadowedScript.transform.parent.GetComponent<DragScript>().myShadow = this;
        for (int i =1; i< heroCharWindowItems.Length; i++)
        {
            heroCharWindowItems[i].GetComponent<ItemSlotScript>().saveCurrentEquip += saveCurrentEquipment;
        }
    }

    void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.3f, 1.0f, 0.3f);
        shadowedScript.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1.0f, 0.5f);
        if(shadowedScript.transform.parent.GetComponent<DragScript>().currentWindowBaseScript != null)
        {
            shadowedScript.transform.parent.GetComponent<DragScript>().currentWindowBaseScript.transform.parent.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1.0f, 0.5f);
        }
    }
    void OnMouseUp()
    {
        ShowStatWindow();

    }
    void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
        if (shadowedScript.transform.parent.GetComponent<DragScript>().currentWindowBaseScript != null)
        {
            shadowedScript.transform.parent.GetComponent<DragScript>().currentWindowBaseScript.transform.parent.GetComponent<SpriteRenderer>().color = Color.white;
        }
        shadowedScript.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Update()
    {
            if (displayStats)
            {
                stats = "Name: " + transform.name +
                "\nLevel: " + shadowedScript.Level +
                "\nXP: " + shadowedScript.XP +
                "\nLife: " + shadowedScript.life +
                "\nMaxLife: " + shadowedScript.maxLife +
                "\nStrength: " + shadowedScript.strength +
                "\nAgility: " + shadowedScript.agility +
                "\nSpeed: " + shadowedScript.speed +
                "\nEndurance: " + shadowedScript.endurance +
                "\nInteligence: " + shadowedScript.inteligence +
                "\nWisdom: " + shadowedScript.wisdom +
                "\nCharisma: " + shadowedScript.charisma;
                panelText.GetComponent<Text>().text = stats;
            if (currentEquipment[0] != null)
            {
                heroCharWindowItems[0].GetComponent<Image>().sprite = currentEquipment[0].GetComponent<SpriteRenderer>().sprite;
            }
        }
    }


    public void ShowStatWindow()
    {

        currentEquipment = shadowedScript.GetComponent<HeroBase>().currentEquipment;
        foreach (GameObject shadow in heroMenu.HerosShadowList)
        {
            if (shadow != this.gameObject)
            {
                shadow.GetComponent<ShadowScript>().displayStats = false;
            }
        }

        if (!panel.activeSelf)
        {
            scrollbar.transform.parent.GetChild(0).GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            displayStats = false;
        }

        displayStats = !displayStats;
        panel.SetActive(displayStats);
        print("change value");


        for (int i = 1; i < currentEquipment.Length; i++)
        {
            if (currentEquipment[i] != null)
            {
                heroCharWindowItems[i].GetComponent<ItemSlotScript>().receiveItem(currentEquipment[i], true);
            }
            else
            {
                heroCharWindowItems[i].GetComponent<ItemSlotScript>().receiveItem(currentEquipment[i], true);
                heroCharWindowItems[i].GetComponent<Image>().enabled = false;
            }
        }

    }

    public delegate void EquipmentSaved();
    public event EquipmentSaved equipmentChanged;


    public void saveCurrentEquipment()
    {
        if (displayStats)
        {
            print("saved");
            for (int i = 1; i < heroCharWindowItems.Length; i++)
            {
                currentEquipment[i] = heroCharWindowItems[i].GetComponent<ItemSlotScript>().itemInTheSlot;
                shadowedScript.GetComponent<HeroBase>().currentEquipment[i] = currentEquipment[i];
            }
        }

        equipmentChanged();
    }
}
