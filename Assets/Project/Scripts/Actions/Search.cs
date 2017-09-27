using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Search : Task {
    int lastIndex = 0, firstIndex = 0;
    Type type;
    ItemPatern item;
    public POI poi;
    Dictionary<Vector3, Vector3> foots = new Dictionary<Vector3, Vector3>();
    int target;
    Go go;
    public Vector3[] path;
    public Search() { }
    public Search(Go _go, int _target)
    {
        target = _target;
        go = _go;
    }
    public Search(Type _type)
    {
        type = _type;
    }
    public Search(ItemPatern _item)
    {
        type = typeof(POIItem);
        item = _item;
    }
    public virtual bool Core(Vector3 _position)
    {
        POI poiT = null;
        if (item != null)
        {
            poiT = POICenter.main.CheckPOI(_position, owner, type, new Dictionary<string, object>() { { "item", item } });
        }
        else
        {
            poiT = POICenter.main.CheckPOI(_position, owner, type);
        }
        if (poiT != null)
        {
            poi = poiT;
            return true;

        }
        return false;
    }
    public override Task Clone()
    {
        Search temp = new Search();
        temp.type = type;
        return temp;
    }
    public override void Start()
    {
        lastIndex = 0;
        firstIndex = 0;
        foots.Clear();
        foots.Add(owner.transform.position, new Vector3(-1, -1, -1));
    }
    public override void Update()
    {
        Vector3 curent;
        for (int i = 0; i < 10; i++)
        {
            if (firstIndex >= foots.Count)
            {
                Failure();
                return;
            }
            curent = foots.Keys.ElementAt(firstIndex);
            firstIndex++;
            ////
            if (Core(curent))
            {
                List<Vector3> path2 = new List<Vector3>();
                Vector3 t3 = curent;
                while (foots[t3] != new Vector3(-1, -1, -1))
                {
                    path2.Add(t3);
                    t3 = foots[t3];
                }
                path = path2.ToArray();
                Success();
                return;
            }
            ////

            if (Vector3.Distance(curent,owner.transform.position) > owner.range)
            {
                Failure();
                return;
            }
            for (int z = 0; z < 3; z++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        Vector3 temp = curent + new Vector3(x - 1, y - 1, z - 1);

                        if (!foots.ContainsKey(temp) && Go.CanGo(temp))
                        {
                            foots.Add(temp, curent);
                               
                        }
                    }
                }
            }
        }

    }

}
public class LookingForNeed : Search
{
    string tag;
    public LookingForNeed(string _tag)
    {
        tag = _tag;
    }
    public override bool Core(Vector3 _position)
    {
            POI poiT = POICenter.main.CheckPOI(_position, owner, null, new Dictionary<string, object>() { { "tag", tag } });

        if (poiT != null)
        {
            poi = poiT;
            return true;

        }
        return false;
    }
}
