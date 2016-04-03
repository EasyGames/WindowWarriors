using UnityEngine;
using System.Collections;

public class PlaceMarker : MonoBehaviour {

    public bool ChangeSize = false;
    GameObject windowCounter;
    
    void Start()
    {
        windowCounter = new GameObject();
        windowCounter.AddComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/3d Text Material");
        windowCounter.AddComponent<TextMesh>().font = Resources.Load<Font>("Font/Arial/ARIAL");
        windowCounter.GetComponent<TextMesh>().fontSize = 120;
        windowCounter.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        windowCounter.name = "window Counter";
        windowCounter.transform.parent = transform;
        transform.GetChild(0).GetComponent<WindowBase>().windowCounter = windowCounter;
    }

    void OnMouseDown()
    {
        ChangeSize = true;
        this.GetComponent<Renderer>().material.color = Color.gray;
    }

    void OnMouseUp()
    {
        this.GetComponent<Renderer>().material.color = Color.white;
    }

}
