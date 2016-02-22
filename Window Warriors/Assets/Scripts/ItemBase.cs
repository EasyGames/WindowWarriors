using UnityEngine;
using System.Collections;

public class ItemBase : MonoBehaviour {


    public enum itemType { Any, Head, Body, Legs, Hand };
    public itemType currentItemType = itemType.Any;

    public heroClass itemClass;

    public int additionalDmg;
}
