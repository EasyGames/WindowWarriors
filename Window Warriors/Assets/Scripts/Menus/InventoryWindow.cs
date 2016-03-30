using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryWindow : MonoBehaviour {

    // Inventory layout variables
    int inventorySlotSize = 24;
    int paddingSize = 2;
    int Margin;

    // Number of horizontal and vertical item slots
    int verticallWindowsNumber;
    int horizontalWindowsNumber;

    // Inventory slot game object, main canvas and content of inventory window
    public GameObject inventorySlot;
    public GameObject content;
    public GameObject canvas;

    // The list of all inventory slots and character slots
    public List<GameObject> characterSlots;
    public List<GameObject> inventorySlots;

    // hero menu
    public HeroMenu heroMenu;
    // Initialization
    void Awake () {

        inventorySlots = new List<GameObject>();
        verticallWindowsNumber = ((int)GetComponent<RectTransform>().rect.height + 20) / (inventorySlotSize + paddingSize);
        horizontalWindowsNumber = ((int)GetComponent<RectTransform>().rect.width - 10) / (inventorySlotSize + paddingSize);
        Margin = ((int)GetComponent<RectTransform>().rect.width - 10) % (inventorySlotSize + paddingSize);
        content.GetComponent<RectTransform>().sizeDelta = new Vector3(content.GetComponent<RectTransform>().sizeDelta.x, (inventorySlotSize + paddingSize) * verticallWindowsNumber + Margin);
        content.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        // Creating the inventory slots background
        for (int y = 0; y < verticallWindowsNumber; y++)
        {
            for (int x = 0; x < horizontalWindowsNumber; x++)
            {
                GameObject tempGO;
                tempGO = Instantiate(inventorySlot);
                tempGO.name = "ItemSlotBG";
                tempGO.transform.SetParent(content.transform);
                tempGO.transform.localScale = new Vector3(1, 1, 1);
                tempGO.GetComponent<Image>().color = Color.gray;
                tempGO.GetComponent<RectTransform>().transform.localPosition = new Vector3(x * inventorySlotSize + paddingSize * x + Margin / 2, y * -inventorySlotSize - paddingSize * y - Margin / 2, 0);
            }
        }

        // creating actual inventory slots;
        for (int y = 0; y< verticallWindowsNumber; y++)
        {
            for(int x = 0; x< horizontalWindowsNumber; x++)
            {
                GameObject tempGO;
                tempGO = Instantiate(inventorySlot);
                tempGO.transform.SetParent(content.transform);
                tempGO.transform.localScale = new Vector3(1, 1, 1);
                tempGO.GetComponent<Image>().enabled = false;
                tempGO.GetComponent<RectTransform>().transform.localPosition = new Vector3(x * inventorySlotSize + paddingSize * x + Margin / 2, y * -inventorySlotSize - paddingSize * y - Margin / 2, -1);
                tempGO.AddComponent<BoxCollider>().size = tempGO.GetComponent<RectTransform>().sizeDelta;
                tempGO.GetComponent<BoxCollider>().center = new Vector3(tempGO.GetComponent<RectTransform>().sizeDelta.x / 2, -tempGO.GetComponent<RectTransform>().sizeDelta.y / 2);
                tempGO.AddComponent<ItemSlotScript>().canvas = canvas;
                tempGO.GetComponent<ItemSlotScript>().scrolling = transform.GetChild(1).gameObject;
                tempGO.GetComponent<ItemSlotScript>().inventoryWindow = this;
                tempGO.GetComponent<ItemSlotScript>().heroMenu = heroMenu;
                tempGO.tag = "ItemSlot";
                inventorySlots.Add(tempGO);
            }
        }

        Canvas.ForceUpdateCanvases();
    }
	
    // Check if the inventory is full
    public bool isInventoryFull()
    {
        int itemsCount = 0;

        foreach (GameObject slot in inventorySlots)
        {
            if (slot.GetComponent<ItemSlotScript>().itemInTheSlot != null)
            {
                itemsCount++;
            }
        }

        if (itemsCount >= inventorySlots.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Sherch for first Empty inventory slot returns null if there is no empty space
    public GameObject firstEmptyInventorySlot()
    {
        foreach(GameObject slot in inventorySlots)
        {
            if (slot.GetComponent<ItemSlotScript>().itemInTheSlot == null)
            {
                return slot;
            }
        }

        return null;
    }

    // Scrollweel the inventory menu;
    void OnMouseOver()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.GetChild(1).GetComponent<Scrollbar>().value += Input.GetAxis("Mouse ScrollWheel");
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.GetChild(1).GetComponent<Scrollbar>().value += Input.GetAxis("Mouse ScrollWheel");
        }
    }


    // Add a row of inventory slots to inventory, at the very bottom.
    public void addInventoryRow()
    {
        // Creating the inventory slots background
        for (int y = verticallWindowsNumber; y < verticallWindowsNumber + 1; y++)
        {
            for (int x = 0; x < horizontalWindowsNumber; x++)
            {
                GameObject tempGO;
                tempGO = Instantiate(inventorySlot);
                tempGO.name = "ItemSlotBG";
                tempGO.transform.SetParent(content.transform);
                tempGO.transform.localScale = new Vector3(1, 1, 1);
                tempGO.GetComponent<Image>().color = Color.gray;
                tempGO.GetComponent<RectTransform>().transform.localPosition = new Vector3(x * inventorySlotSize + paddingSize * x + Margin / 2, y * -inventorySlotSize - paddingSize * y - Margin / 2, 0);
            }
        }

        // creating actual inventory slots;
        for (int y = verticallWindowsNumber; y < verticallWindowsNumber + 1; y++)
        {
            for (int x = 0; x < horizontalWindowsNumber; x++)
            {
                GameObject tempGO;
                tempGO = Instantiate(inventorySlot);
                tempGO.transform.SetParent(content.transform);
                tempGO.transform.localScale = new Vector3(1, 1, 1);
                tempGO.GetComponent<Image>().enabled = false;
                tempGO.GetComponent<RectTransform>().transform.localPosition = new Vector3(x * inventorySlotSize + paddingSize * x + Margin / 2, y * -inventorySlotSize - paddingSize * y - Margin / 2, -1);
                tempGO.AddComponent<BoxCollider>().size = tempGO.GetComponent<RectTransform>().sizeDelta;
                tempGO.GetComponent<BoxCollider>().center = new Vector3(tempGO.GetComponent<RectTransform>().sizeDelta.x / 2, -tempGO.GetComponent<RectTransform>().sizeDelta.y / 2);
                tempGO.AddComponent<ItemSlotScript>().canvas = canvas;
                tempGO.GetComponent<ItemSlotScript>().scrolling = transform.GetChild(1).gameObject;
                tempGO.GetComponent<ItemSlotScript>().inventoryWindow = this;
                tempGO.GetComponent<ItemSlotScript>().heroMenu = heroMenu;
                tempGO.tag = "ItemSlot";
                inventorySlots.Add(tempGO);
            }
        }

        verticallWindowsNumber++;
        content.GetComponent<RectTransform>().sizeDelta = new Vector3(content.GetComponent<RectTransform>().sizeDelta.x, (inventorySlotSize + paddingSize) * verticallWindowsNumber + Margin);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.I))
        {
            content.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            isInventoryFull();
            addInventoryRow();
        }
	
	}


}
