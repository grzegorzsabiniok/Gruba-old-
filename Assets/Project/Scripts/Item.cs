using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Item {
    public static Dictionary<string, ItemPatern> index = new Dictionary<string, ItemPatern>();

    public ItemPatern patern;
    public Transform mesh;
    public Transform container;
    int quality;
    public Item(ItemPatern _patern)
    {
        patern = _patern;
    }
    public Item(string _patern)
    {
        patern = index[_patern];
    }
    public bool HaveTag(string _tag)
    {
        return patern.tags.Contains(_tag);
    }
    public string GetProperties(string _name)
    {
        if (patern.properties.ContainsKey(_name))
        {
            return patern.properties[_name];
        }
        else
        {
            return null;
        }
    }
    public static ItemPatern GetPatern(string _name)
    {
        if (index.ContainsKey(_name))
        {
            return index[_name];
        }
        else
        {
            return null;
        }
    }
    public void DestroyItem()
    {
        GameObject.Destroy(mesh.gameObject);
    }
    public void Plant()
    {
        Interface.main.plant = patern.name;
    }
    public void Place()
    {
        MonoBehaviour.print("nawet dziala");
        Interface.main.structure = MonoBehaviour.Instantiate(Structure.index["carpenter"]);
        Interface.main.structure.position = Vector3.zero;
    }
}
