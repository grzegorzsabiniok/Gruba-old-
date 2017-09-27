using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StocpileSelection : Selection {
    public Transform StockpilePrefab;
    public override void DoThings()
    {
        Transform temp = Transform.Instantiate(StockpilePrefab);
        temp.GetComponent<Stockpile>().AddSlots(start, end);
        Destroy(gameObject);
    }
}
