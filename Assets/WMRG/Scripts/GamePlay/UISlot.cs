using UnityEngine;

public class UISlot : MonoBehaviour
{
    public GameObject UITile;

    void Start()
    {
        UITile.GetComponent<UITile>().UISlot = gameObject;
    }

    void OnMouseDown()
    {
        //Debug.Log("jddkjsdbksjdndksjdhskdj......."+ this.gameObject.name);
    }
}