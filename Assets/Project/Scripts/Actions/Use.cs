using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Use : Task {
    public Transform target;
    bool succes = true;
    float animationTime = 0;
    public Item item;
public Use(Transform _target)
    {
        target = _target;
    }
    public override void Start()
    {
        if(target == null)
        {
            Failure();
            return;
        }
        if (Main.Normalize(owner.transform.position) == Main.Normalize(target.GetComponent<Structure>().interaction.position))
        {
            owner.transform.rotation = target.GetComponent<Structure>().interaction.rotation;
            owner.SetAnimation(target.GetComponent<Structure>().useAnimation);
            owner.UseItem(Item.GetPatern(target.GetComponent<Structure>().neededItem));
        }
    }
    public override void Update()
    {
        if (target == null)
        {
            return;
        }
        if (target.GetComponent<Structure>().neededItem != string.Empty)
        if (owner.CheckItem(Item.GetPatern(target.GetComponent<Structure>().neededItem)) == -1)
        {
                Failure();
                return;
        }
        if (Main.Normalize(owner.transform.position) == Main.Normalize(target.GetComponent<Structure>().interaction.position))
        {
            
            animationTime += Time.deltaTime*Main.main.gameSpeed;
            if (animationTime > target.GetComponent<Structure>().timeAnimation)
            {
                Dictionary<string, object> temp = target.GetComponent<Structure>().Use(owner);
                if(temp !=null)
                if (temp.ContainsKey("item")) {
                    item = (Item)temp["item"];
                }
                owner.SetAnimation("idle");
                owner.ItemToBackpack();
                Success();
                return;
            }
        }
        else
        {
            Failure();
            return;
        }
        
    }
    public override void Failure()
    {
        base.Failure();
    }
}
