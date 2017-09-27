using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPart : Structure,POICaler {

    public Mesh emptyPlant;
    public Material done,empty;
    float growTime = 0;
    public Plant plant,targetPlant;
    int growStage =-1;
    float repeattime;
    Field field;    
    public void Deselect()
    {
        if(growStage == -1)
        {
            transform.Find("default").GetComponent<MeshRenderer>().material = empty;
        }
        else
        {
            transform.Find("default").GetComponent<MeshRenderer>().material = done;
        }
    }
    public override Dictionary<string, object> Use(Unit _user)
    {
        Dictionary<string, object> temp = null ;
        if(growStage == -1)
        {
            transform.Find("default").GetComponent<MeshFilter>().mesh = emptyPlant;
            transform.Find("default").GetComponent<MeshRenderer>().material = done;
            growStage = 0;
            if(targetPlant != null)
            {
                Sow();
            }
        }
        else
        {
            if(targetPlant == null)
            {
                transform.Find("default").GetComponent<MeshFilter>().mesh = emptyPlant;
                if (plant != null)
                {
                    _user.AddItem(new Item(plant.revard));
                    _user.AddItem(new Item(plant.revard));
                }
                plant = null;
            }
            else
            {
                if (plant != null)
                {
                    _user.AddItem(new Item(plant.revard));
                }
                plant = targetPlant;
                transform.Find("default").GetComponent<MeshFilter>().mesh = plant.stages[0];
                growStage = 0;
            }
        }
        return temp;
    }
    public override void Begin()
    {
        //targetPlant = new Plant(10, 1, 3, "carot", Main.itemIndex["carot"]);
        field = transform.GetComponent<Field>();
        Vector3 temp2 = transform.position;
        Task temp = new Multi(new Task[]
        {
            new Go(temp2),
            new Use(transform)
        });
        POICenter.main.AddPOI(new POIWork(Item.index["hoe"],temp), transform.position);
    }
    void Update()
    {
        if(repeattime > 0)
        {
            repeattime -= Time.deltaTime*Main.main.gameSpeed;
        }
        else
        {
            if (repeattime < 0)
            {
                Sow();
                repeattime=0;
            }
        }
        if (plant != null)
        {
            if (growStage < plant.stages.Length)
            {
                if (growTime > plant.growTime)
                {

                    transform.Find("default").GetComponent<MeshFilter>().mesh = plant.stages[growStage];
                    growStage++;
                    growTime = 0;
                }
            }
            else
            {
                if (growStage == plant.stages.Length)
                {
                    Sow();
                    growStage++;
                }
            }
                growTime += Time.deltaTime * Main.main.gameSpeed;
        }
    }
    public void ChangePlant(string _plant)
    {
        if (_plant == null)
        {
            targetPlant = null;
        }
        else
        {
            targetPlant = Plant.GetPlant(_plant);
            if (plant == null && growStage != -1)
            {
                Sow();
            }
        }
    }
    void Sow()
    {
        
        Vector3 temp2 = transform.position;
        Search search = new Search(targetPlant.revard);
        Take take = new Take(search);
        Task temp;
        if (plant != targetPlant)
        {
            temp = new Multi(new Task[]
        {
            search,
            new Go(search),
            take,
            new Go(transform.position),
            new InsertToStructure(this,take),
            new Use(transform)
        });
        }
        else
        {
            temp = new Multi(new Task[]
            {
            new Go(transform.position),
            new Use(transform)
            });
        }
        POICenter.main.AddPOI(new POIWork(Item.index["hoe"], temp), transform.position,this);
    }
    public void Fail(POI _poi)
    {
        repeattime = 10;
    }

}

public class Plant
{
    public static Dictionary<string, Plant> index;
    public float growTime;
    public Mesh[] stages;
    public string name;
    public ItemPatern revard;
    public Plant(string _name)
    {
        name = _name;
        revard = Item.GetPatern(_name);
    }
    public static Plant GetPlant(string _name)
    {
        return index[_name];
    }
}
