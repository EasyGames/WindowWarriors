using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemSlotScript : ItemBase {

    // Dragging variables
    bool dragging = false;
    Vector3 originalPosition;
    float distance;
    GameObject myParent;
    public GameObject canvas;
    LayerMask heroLayer = 1 << 8;
    LayerMask ignoreHeroLayer;

    // this rect treansform
    RectTransform thisRectTransform;

    // Item in this slot and image of slot and inventory
    public InventoryWindow inventoryWindow;
    public GameObject itemInTheSlot;
    Image thisImage;
    bool bisSlotOccupied = false;

    //scrolling
    public GameObject scrolling;

    public delegate void itemChangedSlot();
    public event itemChangedSlot saveCurrentEquip;

    // heroMenu

    public HeroMenu heroMenu;


    // initialization
    void Awake () {
        thisRectTransform = GetComponent<RectTransform>();
        distance = Vector3.Distance(Camera.main.transform.position, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0));
        thisImage = GetComponent<Image>();
        ignoreHeroLayer = ~heroLayer;
    }

    // beggining the drag
    void OnMouseDown()
    {
        originalPosition = transform.position;
        myParent = transform.parent.gameObject;
        transform.SetParent(canvas.transform);
        dragging = true;
        this.gameObject.layer = 8;
    }

    // ending the drag and checking for item swap
    void OnMouseUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, ignoreHeroLayer))
        {
            this.gameObject.layer = 0;
            if (hit.collider.tag == "ItemSlot")
            {
                ItemSlotScript otherSlot = hit.collider.GetComponent<ItemSlotScript>();
                if (this.isSlotEmpty())
                {
                }
                else if(otherSlot.isSlotEmpty() 
                    && (itemInTheSlot.GetComponent<ItemBase>().currentItemType == otherSlot.currentItemType 
                    || otherSlot.currentItemType == itemType.Any))
                {
                    if (otherSlot.currentItemType == itemType.Any)
                    {
                        otherSlot.receiveItem(itemInTheSlot);
                        clearItemSlot();
                    }
                    else if(otherSlot.currentItemType != itemType.Any
                        && (heroMenu.currentlySlecetedShadow().GetComponent<ShadowScript>().shadowedScript.myClass == itemInTheSlot.GetComponent<ItemBase>().itemClass
                        || itemInTheSlot.GetComponent<ItemBase>().itemClass == heroClass.All))
                    {
                        otherSlot.receiveItem(itemInTheSlot);
                        clearItemSlot();
                    }
                }

                else if(!otherSlot.isSlotEmpty() 
                    && (itemInTheSlot.GetComponent<ItemBase>().currentItemType == otherSlot.currentItemType || otherSlot.currentItemType == itemType.Any) 
                    && (currentItemType == otherSlot.itemInTheSlot.GetComponent<ItemBase>().currentItemType || currentItemType == itemType.Any))
                {
                    print(heroMenu.currentlySlecetedShadow().GetComponent<ShadowScript>().shadowedScript.myClass);
                    if (otherSlot.currentItemType == itemType.Any 
                        && otherSlot.itemInTheSlot.GetComponent<ItemBase>().itemClass == heroMenu.currentlySlecetedShadow().GetComponent<ShadowScript>().shadowedScript.myClass)
                    {
                        GameObject otherSlotItem;
                        otherSlotItem = otherSlot.itemInTheSlot;
                        otherSlot.receiveItem(itemInTheSlot);
                        this.receiveItem(otherSlotItem);
                    }
                    else if (otherSlot.currentItemType != itemType.Any
                        && (heroMenu.currentlySlecetedShadow().GetComponent<ShadowScript>().shadowedScript.myClass == itemInTheSlot.GetComponent<ItemBase>().itemClass
                        || itemInTheSlot.GetComponent<ItemBase>().itemClass == heroClass.All))
                    {
                        GameObject otherSlotItem;
                        otherSlotItem = otherSlot.itemInTheSlot;
                        otherSlot.receiveItem(itemInTheSlot);
                        this.receiveItem(otherSlotItem);
                    }
                    
                }
                if (saveCurrentEquip != null)
                {
                    // save the current hero equipment
                    saveCurrentEquip();
                }
                

            }
        }
        dragging = false;
        transform.SetParent(myParent.transform);
        transform.position = originalPosition;

    }
	
	// Update is called once per frame
	void Update () {
            
	    if (dragging)
        {
            Vector3 position = new Vector3(Input.mousePosition.x - thisRectTransform.rect.width/2, Input.mousePosition.y + thisRectTransform.rect.height/2, distance - 10);
            transform.position = Camera.main.ScreenToWorldPoint(position);
        }
	}

    // check if there is any item in the slot
    public bool isSlotEmpty()
    {
        if (itemInTheSlot == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Receive an item for this slot
    public void receiveItem(GameObject itemForTheSLot, bool isFirstTime = false)
    {
        itemInTheSlot = itemForTheSLot;
        if (itemInTheSlot != null)
        { 
            thisImage.sprite = itemForTheSLot.GetComponent<SpriteRenderer>().sprite;
            thisImage.enabled = true;
        }
        if (saveCurrentEquip != null && !isFirstTime)
        {
            saveCurrentEquip();
        }
    }

    // clear the slot of curent item
    public void clearItemSlot()
    {
        itemInTheSlot = null;
        thisImage.sprite = null;
        thisImage.enabled = false;
    }

    void OnMouseOver()
    {
        if (scrolling != null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                scrolling.GetComponent<Scrollbar>().value += Input.GetAxis("Mouse ScrollWheel");
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                scrolling.GetComponent<Scrollbar>().value += Input.GetAxis("Mouse ScrollWheel");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (currentItemType != itemType.Any)
            {
                if (!this.isSlotEmpty())
                {
                    if (!inventoryWindow.isInventoryFull())
                    {
                        GameObject inventorySlot;
                        inventorySlot = inventoryWindow.firstEmptyInventorySlot();
                        if (inventorySlot != null)
                        {
                            inventorySlot.GetComponent<ItemSlotScript>().receiveItem(itemInTheSlot);
                        }
                        clearItemSlot();
                    }
                    else
                    {
                        inventoryWindow.addInventoryRow();
                        GameObject inventorySlot;
                        inventorySlot = inventoryWindow.firstEmptyInventorySlot();
                        if (inventorySlot != null)
                        {
                            inventorySlot.GetComponent<ItemSlotScript>().receiveItem(itemInTheSlot);
                        }
                        clearItemSlot();
                    }
                    saveCurrentEquip();
                }
            }
            else
            {
                equipReplaceItem(itemInTheSlot);
            }

        }

    }

    public void equipReplaceItem(GameObject item)
    {
        if (item != null)
        {
            foreach (GameObject charSlot in inventoryWindow.characterSlots)
            {
                if ((item.GetComponent<ItemBase>().currentItemType == charSlot.GetComponent<ItemSlotScript>().currentItemType)
                    && charSlot.GetComponent<ItemSlotScript>().isSlotEmpty()
                    && ( itemInTheSlot.GetComponent<ItemBase>().itemClass == heroClass.All
                    ||heroMenu.currentlySlecetedShadow().GetComponent<ShadowScript>().shadowedScript.myClass == itemInTheSlot.GetComponent<ItemBase>().itemClass))
                {
                    charSlot.GetComponent<ItemSlotScript>().receiveItem(item);
                    clearItemSlot();
                    return;
                }
            }

            // in case the previous loop fails to find empty slot
            foreach (GameObject charSlot in inventoryWindow.characterSlots)
            {
                if (item.GetComponent<ItemBase>().currentItemType == charSlot.GetComponent<ItemSlotScript>().currentItemType
                    && (itemInTheSlot.GetComponent<ItemBase>().itemClass == heroClass.All
                    || heroMenu.currentlySlecetedShadow().GetComponent<ShadowScript>().shadowedScript.myClass == itemInTheSlot.GetComponent<ItemBase>().itemClass))
                {
                    GameObject otherSlotItem;
                    otherSlotItem = charSlot.GetComponent<ItemSlotScript>().itemInTheSlot;
                    charSlot.GetComponent<ItemSlotScript>().receiveItem(item);
                    this.receiveItem(otherSlotItem);
                    return;
                }
            }
        }
    }
}
