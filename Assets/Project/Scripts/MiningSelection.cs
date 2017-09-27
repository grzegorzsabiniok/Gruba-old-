using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MiningSelection : Selection {
    public Transform minePrefab,designationContainer;
    
    public override void DoThings()
    {
        designationContainer = GameObject.Find("DesignationContainer").transform;
        Vector3 min = new Vector3(Mathf.Min(start.x, end.x), Mathf.Min(start.y, end.y), Mathf.Min(start.z, end.z));
        Vector3 max = new Vector3(Mathf.Max(start.x, end.x), Mathf.Max(start.y, end.y), Mathf.Max(start.z, end.z));
        for (int x = (int)min.x; x <= (int)max.x; x++)
        {
            for (int y = (int)min.y; y <= (int)max.y-1; y++)
            {
                for (int z = (int)min.z; z <= (int)max.z; z++)
                {
                    // Main.main.SetBlock(new Vector3(x, y, z), 0);
                    if (Main.main.GetBlock(new Vector3(x, y, z)) > 1)
                    {
                        if (!designationContainer.gameObject.GetComponentsInChildren<Mine>().Any(t => t.transform.position == new Vector3(x, y + 0.5f, z)))
                        {
                            Transform temp = Transform.Instantiate(minePrefab);
                            temp.position = new Vector3(x, y + 0.5f, z);
                            temp.SetParent(designationContainer);
                            temp.GetComponent<Mine>().Create();
                        }
                    }

                }
            }
        }
        Destroy(gameObject);
    }
    public override bool CanPlace(Vector3 start, Vector3 end)
    {
        return true;
    }
}
