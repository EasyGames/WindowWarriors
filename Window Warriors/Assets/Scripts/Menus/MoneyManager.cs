using UnityEngine;
using System.Collections;

public class MoneyManager : MonoBehaviour {

    Vector3 position;
    Vector3 textPosition;
    int Gold;

    void Start()
    {
        Gold = 0;
        position = new Vector3(10, Screen.height- 40, 30);
        position = Camera.main.ScreenToWorldPoint(position);
        transform.position = position;
        textPosition = transform.position + Vector3.right*1.2f;
        textPosition = Camera.main.WorldToScreenPoint(textPosition);
    }

    void OnGUI()
    {
        GUI.Box(new Rect(textPosition.x, Screen.height- textPosition.y -20, 100, 20), Gold.ToString());
    }

    public void addGold(int Ammount)
    {
        Gold += Ammount;
    }

    public void removeGold(int Amount)
    {
        Gold -= Amount;
    }
}
