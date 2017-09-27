using UnityEngine;
using System.Collections.Generic;
using System.Linq;

class Container : Structure
{
    List<Item> items = new List<Item>();
    int maxCount;

    public override void Insert(Item _item)
    {
        if (items.Count < maxCount)
        {
            items.Add(_item);
        }
    }
    public Item TakeItem(ItemPatern item)
    {
        Item temp = items.First(x => x.patern == item);
        return temp;
    }
}

