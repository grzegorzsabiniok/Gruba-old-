using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartExpedition : MonoBehaviour
{

    public Structure[] structures;
    public Unit[] units;
    public Material good, wrong;
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            if (hit.transform.tag == "terrain")
            {
                transform.position = Main.Normalize(hit.point - (hit.point - ray.origin) / 1000);
            }
            if (CanPlace())
            {

                if (Input.GetMouseButtonDown(0))
                {
                    for (int i = 0; i < units.Length; i++)
                    {
                        units[i].Born();
                    }
                    while (transform.childCount > 0)
                    {
                        transform.GetChild(0).SetParent(null);
                    }
                    Destroy(gameObject);
                }
            }

        }
    }
    bool CanPlace()
    {
        for (int i = 0; i < structures.Length; i++)
        {
            if (!structures[i].CanPlace()) return false;
        }
        for (int i = 0; i < units.Length; i++)
        {
            if (!Go.CanGo(units[i].transform.position)) return false;
        }
        return true;
    }
    public void Begin()
    {
        Instantiate(transform);
    }
}
