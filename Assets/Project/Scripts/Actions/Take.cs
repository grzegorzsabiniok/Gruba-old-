using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Take : Task {
    POIItem poi;
    Search search;
    float animationTime;
    public Item item;
    public Take(Search _search)
    {
        search = _search;
    }
    public override void Start()
    {
        owner.SetAnimation("chop");
        poi = (POIItem)search.poi;
    }
    public override void Update()
    {
        if (Main.Normalize(owner.transform.position) == poi.position)
        {

            animationTime += Time.deltaTime*Main.main.gameSpeed;
            if (animationTime > 2)
            {
                owner.SetAnimation("idle");
                item = poi.item.item;
                owner.AddItem(poi.item.item);
                Willage.willage.RemoveItem(poi.item.item);
                poi.item.Delete();
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
}
