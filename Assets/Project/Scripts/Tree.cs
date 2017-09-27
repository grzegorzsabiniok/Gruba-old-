using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Structure {
    public Transform trunk;
    public ItemPatern wood;
    public override Dictionary<string, object> Use(Unit _user)
    {
        Item temp2 = new Item("wood");
        _user.AddItem(temp2);
        Transform temp = Transform.Instantiate(trunk);
        temp.position = transform.position;
        
        DeleteColiders();
        temp.GetComponent<Structure>().GenerateColiders();
        Destroy(gameObject);
        Dictionary<string, object> temp3 = new Dictionary<string, object>();
        temp3.Add("item", temp2);
        return temp3;
    }
    public override void Interact()
    {
        Search searchStockpile = new Search(typeof(POIStockpile));
        Use use = new Use(transform);
        Task chop = new Multi(new Task[]
        {
            new Go(Main.Normalize(interaction.position)),
            use,
            searchStockpile,
            new Go(searchStockpile),
            new InsertToStockpile(searchStockpile,use)
            //new Drop(searchStockpile,0),
        });
            
        POICenter.main.AddPOI(new POIWork(Item.index[neededItem], chop),interaction.position);
        
    }
}
