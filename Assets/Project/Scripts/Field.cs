using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {

    public Transform fieldPrefab;
    public Dictionary<Vector3, Transform> slots = new Dictionary<Vector3, Transform>();
    public Material select,empty,normal;
    public string plant;
    public void AddSlots(Vector3 start, Vector3 end)
    {
        Vector3 min = new Vector3(Mathf.Min(start.x, end.x), 0, Mathf.Min(start.z, end.z));
        Vector3 max = new Vector3(Mathf.Max(start.x, end.x), 0, Mathf.Max(start.z, end.z));
        for (int x = (int)min.x; x <= (int)max.x; x++)
        {
            for (int y = (int)min.z; y <= (int)max.z; y++)
            {
                Vector3 temp = Main.Normalize(new Vector3(x, start.y + 1, y));
                Transform temp2 = Transform.Instantiate(fieldPrefab);
                temp2.position = temp;
                temp2.SetParent(transform);
                temp2.GetComponent<WindowManager>().parent = transform.GetComponent<WindowManager>();
                slots.Add(temp, temp2);
                print("robiepole");
            }
        }

}
    public void Select()
    {
        foreach(Transform i in slots.Values)
        {
            i.Find("default").GetComponent<MeshRenderer>().material = select;
        }
    }
    public void Deselect()
    {
        foreach (Transform i in slots.Values)
        {
            i.GetComponent<FieldPart>().Deselect();
        }
    }
    public void ChangePlant(string _plant)
    {
        
        plant = _plant;
        GetComponent<WindowManager>().windowPrefab.GetComponent<Window>().Actualize();
        Deselect();
        foreach (Transform i in slots.Values)
        {
            i.GetComponent<FieldPart>().ChangePlant(_plant);
        }
    }
}
