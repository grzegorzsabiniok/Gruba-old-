using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Role
{
    public Unit owner;
    public string name;
    public ItemPatern[] needItems;
    public virtual void Behaviour() {
        bool t2 = true;
        if (needItems != null)
        {
            for (int i = 0; i < owner.items.Length; i++)
            {
                if (owner.items[i] != null)
                {
                    if (!needItems.Contains(owner.items[i].patern))
                    {
                        Search searchStock = new Search(typeof(POIStockpile));
                        Task temp = new Multi(new Task[]
                        {
                    searchStock,
                    new Go(searchStock),

                        });
                        owner.AddTask(temp);
                        owner.AddTask(new InsertToStockpile(searchStock, owner.items[i]));
                        t2 = false;
                    }
                }
            }
        }
        if (t2)
        {
            Search swork = new Search(typeof(POIWork));
            //owner.AddTask(new Multi(new Task[] { new Drop(), swork }));
            owner.AddTask(new Multi(new Task[] { swork }));
        }
    }
    public virtual void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            if (owner.items[i] != null)
            {
                if (needItems != null)
                {
                    if (!needItems.Contains(owner.items[i].patern))
                    {
                        Search searchStock = new Search(typeof(POIStockpile));
                        Task temp = new Multi(new Task[]
                        {
                searchStock,
                new Go(searchStock),

                        });
                        owner.AddTask(temp);
                        owner.AddTask(new InsertToStockpile(searchStock, owner.items[i]));
                    }
                }
                else
                {
                    Search searchStock = new Search(typeof(POIStockpile));
                    Task temp = new Multi(new Task[]
                    {
                searchStock,
                new Go(searchStock),

                    });
                    owner.AddTask(temp);
                    owner.AddTask(new InsertToStockpile(searchStock, owner.items[i]));
                
            }
            }
        }
        if (needItems != null)
        {
            for (int i = 0; i < needItems.Length; i++)
            {
                if (owner.items.Count(x => x != null && x.patern == needItems[i]) < 1)
                {
                    Search searchStock = new Search(needItems[i]);
                    Task temp = new Multi(new Task[]
                    {
                searchStock,
                new Go(searchStock),
                new Take(searchStock)
                    });
                    owner.AddTask(temp);
                }
            }
        }
    }
}
public class Farmer : Role
{
    public Farmer(Unit _owner)
    {
        owner = _owner;
        name = "Farmer";
        needItems = new ItemPatern[]
        {
            Item.index["hoe"]
        };
    }

}
public class Warrior : Role
{
    public Warrior(Unit _owner)
    {
        owner = _owner;
        name = "Warrior";
        needItems = new ItemPatern[]
        {
            Item.index["shortSword"],Item.index["shield"]
        };
    }
    public override void Behaviour()
    {
        
    }
}
public class Miner : Role
{
    public Miner(Unit _owner)
    {
        owner = _owner;
        name = "Miner";
        needItems = new ItemPatern[]
{
            Item.index["pickaxe"]
};
    }
    public override void Behaviour()
    {
        bool t2 = true;
        for(int i = 0; i < owner.items.Length; i++)
        {
            if(owner.items[i] != null)
            {
                if(!needItems.Contains(owner.items[i].patern))
                {
                    Search searchStock = new Search(typeof(POIStockpile));
                    Task temp = new Multi(new Task[]
                    {
                    searchStock,
                    new Go(searchStock),
                    
                    });
                    owner.AddTask(temp);
                    owner.AddTask(new InsertToStockpile(searchStock, owner.items[i]));
                    t2 = false;
                }
            }
        }
        if (t2)
        {
            Search swork = new Search(typeof(POIWork));
            //owner.AddTask(new Multi(new Task[] { new Drop(), swork }));
            owner.AddTask(new Multi(new Task[] { swork }));
        }
    }
}
public class Woodcutter : Role
{
    public Woodcutter(Unit _owner)
    {
        owner = _owner;
        name = "Woodcutter";
        needItems = new ItemPatern[]
        {
            Item.index["axe"]
        };
    }
    public override void Behaviour()
    {
        bool t2 = true;
        for (int i = 0; i < owner.items.Length; i++)
        {
            if (owner.items[i] != null)
            {
                if (!needItems.Contains(owner.items[i].patern))
                {
                    Search searchStock = new Search(typeof(POIStockpile));
                    Task temp = new Multi(new Task[]
                    {
                    searchStock,
                    new Go(searchStock),

                    });
                    owner.AddTask(temp);
                    owner.AddTask(new InsertToStockpile(searchStock, owner.items[i]));
                    t2 = false;
                }
            }
        }
        if (t2)
        {
            Search swork = new Search(typeof(POIWork));
            //owner.AddTask(new Multi(new Task[] { new Drop(), swork }));
            owner.AddTask(new Multi(new Task[] { swork }));
        }
    }
}
public class Worker : Role
{
    public Worker(Unit _owner)
    {
        owner = _owner;
        name = "Worker";
        needItems = new ItemPatern[0];
    }
}