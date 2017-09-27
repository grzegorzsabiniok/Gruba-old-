using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
public class CraftingWindow : Window
{
    WorkShop owner;
    public Transform queuePrefab, queueContainer, itemsPrefab, itemsContainer, 
        crafting,craftingResources,resourcePrefab,resourceMenu,materialsPanel,
        building;
    public Image craftingIcon;
    public Text craftingTitle,craftingCount;
    ItemPatern actual;
    ItemPatern[] materials;
    public override void Connect(Transform _owner)
    {
        owner = _owner.GetComponent<WorkShop>();
        Actualize();
    }
    public override void Actualize()
    {
        if (owner != null)
        {
            building.gameObject.SetActive(owner.plane);
            if (owner.plane)
            {
                string temp = "Structure need:\n";
                    foreach(string i in owner.resources)
                {
                    temp += "\t-"+ i+"\n";
                }
                building.Find("Text").GetComponent<Text>().text = temp;
            }
            KillChildren(itemsContainer);
            for (int i = 0; i < owner.items.Length; i++)
            {
                Transform temp = Instantiate(itemsPrefab);
                ItemPatern temp2 = Item.index[owner.items[i]];
                temp.SetParent(itemsContainer);
                temp.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { Craft(temp2); });
                temp.Find("Text").GetComponent<Text>().text = temp2.name;
                temp.Find("Icon").GetComponent<Image>().sprite = temp2.icon;
            }
        }
    }
    void KillChildren(Transform _target)
    {
        foreach (Transform child in _target)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void Craft(ItemPatern _patern)
    {
        print("zamawiam "+_patern.name);
        materials = new ItemPatern[_patern.resources.Length];
        actual = _patern;
        crafting.gameObject.SetActive(true);
        craftingIcon.sprite = _patern.icon;
        craftingTitle.text = _patern.name;
        craftingCount.text = "0";
        //Item[] resources = Stockpile.FindItems(_patern.resources[0]);
        KillChildren(craftingResources);
        for(int i = 0; i < _patern.resources.Length; i++)
        {
            int j = i;
            Transform temp = Instantiate(resourcePrefab);
            temp.SetParent(craftingResources);
            temp.GetComponent<Button>().onClick.AddListener(delegate { ChooseResources(j); });
        }
        if (Main.main.GetBlock(owner.interaction.position) <= 0)
        {
            //owner.AddTask(_nr);
            //owner.AddOrder(_patern,)
            /*
            Transform temp = Instantiate(queuePrefab);
            temp.SetParent(queueContainer);
            temp.Find("Main/Text").GetComponent<Text>().text = target.GetComponent<WorkShop>().items[_nr].name;
            */
        }
    }
    public void ChooseResources(int _nr)
    {
        resourceMenu.gameObject.SetActive(true);
        Item[] resources = Stockpile.FindItems(actual.resources[_nr]);
        print(_nr + "materialy " + resources.Length+actual.resources[_nr]);
        KillChildren(materialsPanel);
        for (int i = 0; i < resources.Length; i++)
        {
            int j = i;
            Transform temp = Instantiate(resourcePrefab);
            temp.SetParent(materialsPanel);
            temp.GetComponent<Image>().sprite = resources[i].patern.icon;
            temp.GetComponent<Button>().onClick.AddListener(delegate { Choose(resources[j].patern,_nr); });
        }
    }
    public void Choose(ItemPatern _item, int _nr)
    {
        materials[_nr] = _item;
        craftingResources.GetChild(_nr).GetComponent<Image>().sprite = _item.icon;
        Close(resourceMenu);
    }
    public void Add(int _mod)
    {
        int temp = int.Parse(craftingCount.text) + _mod;
        if (temp < 0) temp = 0;
        craftingCount.text = temp.ToString();
    }
    public void Accept()
    {
        if (!materials.Contains(null) && int.Parse(craftingCount.text) > 0)
        {
            Transform temp = Instantiate(queuePrefab);
            temp.SetParent(queueContainer);
            temp.Find("Main/Text").GetComponent<Text>().text = actual.name + "X" + craftingCount.text;
            owner.AddOrder(actual, materials, int.Parse(craftingCount.text),temp);
            Close(crafting);
        }
        
    }
}
