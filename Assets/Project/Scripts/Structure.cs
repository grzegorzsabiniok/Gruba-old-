using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour,POICaler
{
    public static Dictionary<string, Transform> index;
    public Transform windowPrefab;
    public string useAnimation;
    public float timeAnimation;
    public string neededItem;
    public List<string> resources = new List<string>();
    public bool plane = false;
    public string name;
    public Vector3[] grid;
    public Vector3[] fundations;
    public Transform interaction;
    public float repeatTime;
    public void LoadResources(string[] _res)
    {
        resources = new List<string>(_res);
    }
    void Start()
    {
        Begin();
    }
    public void PlacePlan()
    {
        plane = true;
        GetComponent<WindowManager>().Actualize();
        for(int i=0;i<resources.Count;i++)
        {
                Search searchResources = new Search(Item.GetPatern(resources[i]));
                Take takeResource = new Take(searchResources);

                Task temp = new Multi(new Task[]
                {
                            searchResources,
                            new Go(searchResources),
                            takeResource,
                            new Go(Main.Normalize(interaction.position)),
                            new InsertToStructure(this,takeResource)
                });
                POI temp2 = new POIWork(null, temp);
                POICenter.main.AddPOI(temp2, interaction.position,this);
        }
    }
    public virtual void Insert(Item _item)
    {
        resources.Remove(_item.patern.name);

        if(resources.Count == 0)
        {
            plane = false;
            transform.Find("default").GetComponent<MeshRenderer>().material = Main.main.normalPalete;
        }
        GetComponent<WindowManager>().Actualize();
    }
    public virtual void Begin() { }

    public bool CanPlace()
    {
        for (int i = 0; i < grid.Length; i += 2)
        {
            for (float x = grid[i].x; x <= grid[i + 1].x; x++)
            {
                for (float y = grid[i].y; y <= grid[i + 1].y; y++)
                {
                    for (float z = grid[i].z; z <= grid[i + 1].z; z++)
                    {
                        if (Main.main.GetBlock(transform.position + new Vector3(x, y, z)) != 0)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        if (fundations.Length > 0)
            for (float x = fundations[0].x; x <= fundations[1].x; x++)
            {
                for (float y = fundations[0].y; y <= fundations[1].y; y++)
                {
                    for (float z = fundations[0].z; z <= fundations[1].z; z++)
                    {
                        if (Main.main.GetBlock(transform.position + new Vector3(x, y, z)) == 0)
                        {
                            return false;
                        }
                    }
                }
            }
        return true;
    }
    public void ModifyColiders(int _x)
    {
        for (int i = 0; i < grid.Length; i += 2)
        {
            for (float x = grid[i].x; x <= grid[i + 1].x; x++)
            {
                for (float y = grid[i].y; y <= grid[i + 1].y; y++)
                {
                    for (float z = grid[i].z; z <= grid[i + 1].z; z++)
                    {
                        Main.main.SetBlock(transform.position + new Vector3(x, y, z), _x);
                    }
                }
            }
        }
    }
    public virtual void GenerateColiders()
    {
        ModifyColiders(-1);
        if(GetComponent<WindowManager>())
        GetComponent<WindowManager>().bornTime = Time.time; 
    }
    public void DeleteColiders()
    {
        ModifyColiders(0);
    }
    public void Rotate(int _angle)
    {
        transform.localEulerAngles += new Vector3(0, 90, 0);
        for (int i = 0; i < grid.Length; i += 2)
        {
            float[] temp = new float[4]
            {
                            grid[i].x,grid[i+1].x,grid[i].z,grid[i+1].z
            };
            grid[i].x = temp[2];
            grid[i + 1].x = temp[3];
            grid[i].z = temp[1] * -1;
            grid[i + 1].z = temp[0] * -1;

        }
        float[] temp2 = new float[4]
{
                            fundations[0].x,fundations[1].x,fundations[0].z,fundations[1].z
};
        fundations[0].x = temp2[2];
        fundations[1].x = temp2[3];
        fundations[0].z = temp2[1] * -1;
        fundations[1].z = temp2[0] * -1;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < grid.Length; i += 2)
        {

            Gizmos.DrawCube(transform.position +new Vector3(0,0.5f,0)+ new Vector3((grid[i + 1].x - grid[i].x) / 2.0f + grid[i].x, (grid[i + 1].y - grid[i].y) / 2.0f + grid[i].y, (grid[i + 1].z - grid[i].z) / 2.0f + grid[i].z), new Vector3(grid[i + 1].x - grid[i].x + 1, grid[i + 1].y - grid[i].y + 1, grid[i + 1].z - grid[i].z + 1));

        }

        Gizmos.color = Color.blue;
        if (fundations.Length > 0)
            Gizmos.DrawCube(transform.position + new Vector3(0, 0.5f, 0) + new Vector3((fundations[1].x - fundations[0].x) / 2.0f + fundations[0].x, (fundations[1].y - fundations[0].y) / 2.0f + fundations[0].y, (fundations[1].z - fundations[0].z) / 2.0f + fundations[0].z), new Vector3(fundations[1].x - fundations[0].x + 1, fundations[1].y - fundations[0].y + 1, fundations[1].z - fundations[0].z + 1));


    }
    public virtual Dictionary<string, object> Use(Unit _user)
    {
        return null;
    }
    public virtual void Use(Unit _user, int type)
    {

    }
    public virtual void Interact() { }
    private void Update()
    {
        if(repeatTime > 0)
        {
            repeatTime -= Time.deltaTime;
        }
        else
        {
            if(repeatTime == 0)
            {

            }
            else
            {
                repeatTime = 0;
                for (int i = 0; i < resources.Count; i++)
                {
                    Search searchResources = new Search(Item.GetPatern(resources[i]));
                    Take takeResource = new Take(searchResources);

                    Task temp = new Multi(new Task[]
                    {
                            searchResources,
                            new Go(searchResources),
                            takeResource,
                            new Go(Main.Normalize(interaction.position)),
                            new InsertToStructure(this,takeResource)
                    });
                    POI temp2 = new POIWork(null, temp);
                    POICenter.main.AddPOI(temp2, interaction.position, this);
                }
                print("jeszcze raz");
            }
        }
    }
    public void Fail(POI poi)
    {

    }
}
