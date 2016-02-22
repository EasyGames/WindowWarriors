﻿using UnityEngine;
using System.Collections;

public class PlaceMarker : MonoBehaviour {

    public bool ChangeSize = false;

    void OnMouseDown()
    {
        print("Click");
        ChangeSize = true;
        this.GetComponent<Renderer>().material.color = Color.gray;
    }

    void OnMouseUp()
    {
        this.GetComponent<Renderer>().material.color = Color.white;
    }

}
