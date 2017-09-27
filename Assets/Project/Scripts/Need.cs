using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Need{
    Unit owner;
    public string name;
    public float level;
    protected float mod;
    protected string[] tags;
    public void Start(Unit _owner)
    {
        owner = _owner;
        level = 100;
    }
    public bool Check()
    {
        return (level < 50);
    }
    public virtual void Behaviour()
    {
        if(level > 80)
        {
            owner.actualNeed = null;
            return;
        }
            string actualTag = "";
            if (level < 10) actualTag = tags[0];
            else actualTag = tags[1];
            //in equipment
            for(int i = 0; i < owner.items.Length; i++)
            {
                if (owner.items[i] != null)
                {
                    if (owner.items[i].HaveTag(actualTag))
                    {
                    if (owner.items[i].GetProperties(actualTag) != null)
                        level += int.Parse(owner.items[i].GetProperties(actualTag));
                        owner.RemoveItem(owner.items[i]);
                        return;
                    }
                }
            }
            //search
            Search search = new LookingForNeed(actualTag);
            Task temp = new Multi(new Task[]
            {
                search,
                new Go(search),
                new Take(search)

            });
            owner.AddTask(temp);

        
    }
    public void Update()
    {
        level -= Time.deltaTime * Main.main.gameSpeed * mod;
        if (level < 0) level = 0;
    }
    
}
public class Food : Need
{
    public Food()
    {
        name = "food";
        mod = 0.25f;
        tags = new string[] { "starving", "food" };
    }
}
