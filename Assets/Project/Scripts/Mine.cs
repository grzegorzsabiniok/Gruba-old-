using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour,POICaler
{
    List<POI> pois = new List<POI>();
    public Transform designationContainer;
    Vector3[] positions = new Vector3[]
    {
        new Vector3(1,0,0),
        new Vector3(-1,0,0),
        new Vector3(0,0,1),
        new Vector3(0,0,-1),

        new Vector3(1,1,0),
        new Vector3(-1,1,0),
        new Vector3(0,1,1),
        new Vector3(0,1,-1),

        new Vector3(1,-1,0),
        new Vector3(-1,-1,0),
        new Vector3(0,-1,1),
        new Vector3(0,-1,-1),

        new Vector3(1,-2,0),
        new Vector3(-1,-2,0),
        new Vector3(0,-2,1),
        new Vector3(0,-2,-1),
    };
    public void Create()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 temp2 = Main.Normalize(transform.position + positions[i] - new Vector3(0, 0.5f, 0));
            Search searchStockpile = new Search(typeof(POIStockpile));
            Task mine = new Multi(new Task[]{
                new Go(temp2),
                new Mining(this),
            });
            POI temp = new POIWork(Item.index["pickaxe"], mine);
            pois.Add(temp);
            POICenter.main.AddPOI(temp, temp2,this);
        }
        for(int i = 0; i < pois.Count; i++)
        {
            pois[i].subpois = pois.ToArray();
        }

    }
    public void Use()
    {
        /*
        for (int i = 0; i < pois.Count; i++)
        {
            POICenter.main.RemovePOI(pois[i]);
        }
        */
        Destroy(gameObject);
    }
    public void Fail(POI _poi)
    {
        Destroy(gameObject);
    }
}
