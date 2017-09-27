using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour {
    public Item item;
    public Stockpile stockpile;
    public void Delete()
    {
        Destroy(transform.GetComponent<WindowManager>().windowPrefab.gameObject);
        Destroy(gameObject);
    }
}
