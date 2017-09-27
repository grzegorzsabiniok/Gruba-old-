using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

public class WorkShop : Structure,POICaler {
    public string[] items;
    public List<Item> storage = new List<Item>();
    public List<int> itemQueue;
    public List<Order> orders = new List<Order>();
    public override void Begin()
    {
        /*
        string gridT = "";
        for(int i = 0; i < grid.Length; i++)
        {
            gridT += grid[i].x + "/" + grid[i].y + "/" + grid[i].z + " ";
        }
        print("name: workshop/" + name +
            "\n\tneedItem:" + neededItem +
            "\n\tposition:" +
            "\n\tscale: 0.1" +
            "\n\tinteractionPosition: -1/0/0" +
            "\n\tinteractionRotation: 0/90/0" +
            "\n\tanimation: " + useAnimation +
            "\n\ttime: " + timeAnimation +
            "\n\tgrid: " + gridT +
            "\n\tfundation: " + fundations[0].x + "/" + fundations[0].y + "/" + fundations[0].z + " " + fundations[1].x + "/" + fundations[1].y + "/" + fundations[1].z +
            "\n\tmesh: "+
            "\n\titems: "
            );
            */
    }
    public class Order
    {
        public ItemPatern patern;
        ItemPatern[] resources;
        public POI[] pois;
        int count;
        int faze;
        public Transform prefab;
        public Order(ItemPatern _patern, ItemPatern[] _resources, int _count,Transform _prefab)
        {
            patern = _patern;
            resources = _resources;
            count = _count;
            prefab = _prefab;
            faze = 0;
        }
        public void Start(WorkShop _target)
        {
            List<POI> poisT = new List<POI>();
            List<Item> items = new List<Item>();
            for (int q = 0; q < count; q++)
            {
                for (int i = 0; i < resources.Length; i++)
                {
                    Item t = _target.storage.Find(a => a.patern == resources[i] && !items.Contains(a));
                    if (t != null)
                    {
                        items.Add(t);
                    }
                    else { 

                        Search searchResources = new Search(resources[i]);
                        Take takeResource = new Take(searchResources);

                        Task temp = new Multi(new Task[]
                        {
                            searchResources,
                            new Go(searchResources),
                            takeResource,
                            new Go(Main.Normalize(_target.interaction.position)),
                            new InsertToWorkshop(_target, takeResource)
                        });
                        POI temp2 = new POIWork(null, temp);
                        POICenter.main.AddPOI(temp2, _target.interaction.position, _target);
                        poisT.Add(temp2);
                        
                        
                    }
                }
            }
            pois = poisT.ToArray();
        }
        public void Cancel(WorkShop _target)
        {
            
            Destroy(prefab.gameObject);
            _target.orders.Remove(this);
            if (_target.orders.Count > 0)
            {
                _target.orders[0].Start(_target);
            }
        }
        public bool CheckResources(WorkShop _target)
        {
            List<Item> temp = new List<Item>();
            int temp2 = 0;
            for(int i = 0; i < resources.Length; i++)
            {
                for(int q = 0; q < _target.storage.Count; q++)
                {
                    if(_target.storage[q].patern == resources[i] && !temp.Contains(_target.storage[q]))
                    {
                        temp.Add(_target.storage[q]);
                        temp2++;
                        break;
                    }
                }

            }
            if (temp2 == resources.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void RemoveResources(WorkShop _target)
        {
            for (int i = 0; i < resources.Length; i++)
            {
                _target.storage.Remove(_target.storage.First(a => a.patern == resources[i]));
            }
            count--;
            if (count < 1)
            {
                _target.orders.RemoveAt(0);
                Destroy(prefab.gameObject);
                if(_target.orders.Count > 0)
                {
                    _target.orders[0].Start(_target);
                }
            }
            else
            {
                prefab.Find("Main/Text").GetComponent<Text>().text = patern.name + "X" + count;
            }
        }
    }
    public void AddItem(Item _item)
    {
        storage.Add(_item);
        if(orders.Count > 0)
        if (orders[0].CheckResources(this))
        {
            Task temp = new Multi(new Task[]
            {
                new Go(interaction.position),
                new Use(transform),

            });
            POICenter.main.AddPOI(new POIWork(Item.GetPatern(neededItem), temp), interaction.position);
        }
    }
    public void AddOrder(ItemPatern _patern,ItemPatern[] _resources,int _count,Transform _prefab)
    {
        orders.Add(new Order(_patern, _resources, _count, _prefab));
        if(orders.Count == 1)
        {
            orders[0].Start(this);
        }
    }
    public override Dictionary<string, object> Use(Unit _user)
    {
        Dictionary<string, object> temp = new Dictionary<string, object>();
        Item item = new Item(orders[0].patern);
        temp.Add("item",item);
        _user.AddItem(item);
        orders[0].RemoveResources(this);
        return temp;
    }
    public void Fail(POI poi)
    {
        for(int i = 0; i < orders.Count; i++)
        {
            for(int j = 0; j < orders[i].pois.Length; j++)
            {
                if(orders[i].pois[j] == poi)
                {
                    orders[i].Cancel(this);
                    return;
                }
            }
        }
        repeatTime = Main.main.SecondTryTime;
        print("skaszanilem");
    }
}
