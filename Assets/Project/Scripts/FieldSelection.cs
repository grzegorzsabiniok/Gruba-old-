using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSelection : Selection {
    public Transform fieldPrefab;
    public override void DoThings()
    {
        Transform temp = Transform.Instantiate(fieldPrefab);
        temp.GetComponent<Field>().AddSlots(start, end);
        Destroy(gameObject);
    }
}
