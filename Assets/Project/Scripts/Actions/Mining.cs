using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mining : Task {
    public Mine target;
    bool succes = true;
    float animationTime = 0;
    
    public Mining(Mine _target)
    {
        target = _target;
        name = "Mine";
    }
    public override void Start()
    {
        owner.SetAnimation("chop");
        owner.UseItem(Item.index["pickaxe"]);
        owner.transform.LookAt(target.transform);
        owner.transform.localEulerAngles = new Vector3(0, owner.transform.localEulerAngles.y, 0);
    }
    public override void Update()
    {
            animationTime += Time.deltaTime * Main.main.gameSpeed;
            if (animationTime > 3)
            {
            float temp = Random.Range(0, 100);
            if(temp < 80)
            {
                if(temp < 20)
                owner.AddItem(new Item("stone"));
            }
            else
            {
                owner.AddItem(new Item("ironOre"));
            }
                Main.main.SetBlock(target.transform.position, 0);
                owner.SetAnimation("idle");
                owner.ItemToBackpack();
                target.Use();
                Success();
            }

    }
}
