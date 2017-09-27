using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : Task {
    POI poi;
    Item item;
    Search search;
    Take take;
    WorkShop workshop;
    int mode;
    /*
     0 - ground
     1 - stockpile
     2 - workshop
    */
public Drop()
    {
        mode = 0;
        name = "Drop";
    }
    public Drop(WorkShop _workshop)
    {
        workshop = _workshop;
        mode = 2;
    }
    public Drop(Search _search,Take _take)
    {
        search = _search;
        take = _take;
        mode = 1;
    }
    public override void Start()
    {
        if(search!=null)
        if(search.poi!=null)
        poi = search.poi;
    }
    public override void Update()
    {
        /*
        if (owner.items[item] == null)
        {
            Success();
            return;
        }
        */
        if(take == null)
        {
            
            POICenter.main.AddPOI(new POIItem(owner.DropItem()), owner.transform.position);
            Success();
            return;
        }
        if (take != null)
        {
            item = take.item;
        }
        ItemContainer temp = owner.DropItem(item);
        if(temp == null)
        {

        }
        if(poi != null)
        {
            ((POIStockpile)poi).stockpile.slots[owner.transform.position] = item;
        }
        
        POICenter.main.AddPOI(new POIItem(temp), owner.transform.position);
        
        //Main.main.SetBlock(task.owner.transform.position, 0);
        owner.SetAnimation("idle");
        Success();
        return;
    }
}
public class InsertToStockpile : Task
{
    Search stockpile;
    Take take;
    Use use;
    Item item;
    public InsertToStockpile(Search _stockpile, Take _take)
    {
        stockpile = _stockpile;
        take = _take;
        name = "Drop";
    }
    public InsertToStockpile(Search _stockpile, Use _use)
    {
        stockpile = _stockpile;
        use = _use;
        name = "Drop";
    }
    public InsertToStockpile(Search _stockpile, Item _item)
    {
        stockpile = _stockpile;
        item = _item;
    }
    public override void Update()
    {
        Item temp = null;

        if (use != null)
        {
            temp = use.item;
        }
        if(take != null)
        {
            temp = take.item;
        }
        if (item != null)
        {
            temp = item;
        }
        if (stockpile.poi != null)
        {
            ((POIStockpile)stockpile.poi).stockpile.slots[owner.transform.position] = temp;
            ((POIStockpile)stockpile.poi).stockpile.AddItem(temp);
        }
        POICenter.main.AddPOI(new POIItem(owner.DropItem(temp)), owner.transform.position);
        owner.SetAnimation("idle");
        Success();
        return;
    }
}
public class InsertToWorkshop : Task
{
    WorkShop workshop;
    Take take;
    public InsertToWorkshop(WorkShop _workshop, Take _take)
    {
        workshop = _workshop;
        take = _take;
        name = "Drop";
    }
    public override void Update()
    {
        workshop.AddItem(take.item);
        owner.RemoveItem(take.item);
        //POICenter.main.AddPOI(new POIItem(owner.DropItem(take.item)), owner.transform.position);
        owner.SetAnimation("idle");
        Success();
        return;
    }
}
public class InsertToStructure : Task
{
    Structure structure;
    Take take;
    public InsertToStructure(Structure _structure, Take _take)
    {
        structure = _structure;
        take = _take;
        name = "Drop";
    }
    public override void Update()
    {
        structure.Insert(take.item);
        owner.RemoveItem(take.item);
        //POICenter.main.AddPOI(new POIItem(owner.DropItem(take.item)), owner.transform.position);
        owner.SetAnimation("idle");
        Success();
        return;
    }
}
