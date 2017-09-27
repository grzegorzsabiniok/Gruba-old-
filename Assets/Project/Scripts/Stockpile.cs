using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Stockpile : MonoBehaviour {

    public Transform prefab;
    public static List<Stockpile> stockpileIndex;
    public static Dictionary<ItemPatern, int> itemIndex;
    public Dictionary<Vector3, Item> slots = new Dictionary<Vector3, Item>();
    public void AddSlots(Vector3 start, Vector3 end)
    {
        Vector3 min = new Vector3(Mathf.Min(start.x, end.x), 0, Mathf.Min(start.z, end.z));
        Vector3 max = new Vector3(Mathf.Max(start.x, end.x), 0, Mathf.Max(start.z, end.z));
        print(start.y);
        for (int x = (int)min.x; x <= (int)max.x; x++)
        {
            for (int y = (int)min.z; y <= (int)max.z; y++)
            {
                Vector3 temp = Main.Normalize(new Vector3(x, start.y+1, y));
                slots.Add(temp, null);
                Main.main.SetBlock(temp, -100);
                POICenter.main.AddPOI(new POIStockpile(this), temp);
            }
        }
        Show();
    }
    public void AddItem(Item _item)
    {
        if (itemIndex.ContainsKey(_item.patern))
        {
            itemIndex[_item.patern]++;
        }
        else
        {
            itemIndex.Add(_item.patern, 1);
        }
    }
    public void RemoveItem(Item _item)
    {
        foreach(Vector3 i in slots.Keys)
        {
            if(slots[i] == _item)
            {
                slots[i] = null;
                return;
            }
        }
    }
    public void Show()
    {
        foreach(Vector3 i in slots.Keys)
        {
            Transform temp = Transform.Instantiate(prefab);
            temp.position = i;

        }
    }
    public void Start()
    {
        Willage.willage.stockpiles.Add(this);
        stockpileIndex.Add(this);
    }
    public static Item[] FindItems(string tag)
    {
        List<Item> temp = new List<Item>();
        for(int i = 0; i < stockpileIndex.Count; i++)
        {
            foreach(Item item in stockpileIndex[i].slots.Values)
            {
                if(item != null)
                if (item.patern.tags.Contains(tag))
                {
                    temp.Add(item);
                }
            }
        }
        return temp.ToArray();
    }
}
