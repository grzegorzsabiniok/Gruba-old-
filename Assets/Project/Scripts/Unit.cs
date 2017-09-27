using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Unit : MonoBehaviour {
    public Sprite portrait;
    public string species;
    public string[] startItems;
    public string startRole;
    public List<Task> tasks = new List<Task>();
    public float speed = 1;
    public int range;
    public Role role;
    public bool ready = false;
    public static int count=0;
    public Camera eye;
    public Dictionary<string, Need> needs = new Dictionary<string, Need>();
    public Need actualNeed;
    // info
    public string infoTitle, infoDesc;
    bool searching = false;
    public string firstName, lastName;
    //equipment
    public Item[] items = new Item[5];
    void Start()
    {
        needs.Add("food", new Food());
        needs["food"].Start(this);
    }
    void Update()
    {
        
        if (ready)
        {
            foreach(Need need in needs.Values)
            {
                need.Update();
            }
            GetComponent<BoxCollider>().enabled = true;
            while (!Go.CanGo(Main.Normalize(transform.position)))
            {
                transform.position -= new Vector3(0, 1, 0);
            }
            if (tasks.Count > 0)
            {
                tasks[0].Behaviour();
            }
            else
            {
                if (actualNeed != null)
                {
                    actualNeed.Behaviour();
                }
                else
                {
                    foreach (Need need in needs.Values)
                    {
                        if (need.Check())
                        {
                            actualNeed = need;
                        }
                    }
                    //praca
                    if(actualNeed == null)
                    role.Behaviour();
                }
            }
        }
        else
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    public void RemoveItem(ItemPatern _item)
    {
        GameObject.Destroy(items[0].mesh.gameObject);
        items[0] = null;
    }
    public void RemoveItem(Item _item)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == _item)
            {
                Destroy(items[i].mesh.gameObject);
                items[i] = null;
                transform.GetComponent<WindowManager>().windowPrefab.GetComponent<Window>().Actualize();
                return;
            }
        }

    } 
    public void AddItem(Item _item)
    {
        if (_item.patern.size == 0)
        {
            for (int i = 1; i < 5; i++)
            {
                if (items[i] == null)
                {
                    items[i] = _item;
                    items[i].patern.Put(items[i], this);
                    break;
                }
            }
        }
        else
        {
            if (items[0] == null)
            {
                items[0] = _item;
                items[0].patern.Put(items[0], this);
            }
        }
        transform.GetComponent<WindowManager>().windowPrefab.GetComponent<Window>().Actualize();
    }
    public int CheckItem(ItemPatern _patern)
    {
        int temp = -1;
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] != null)
            if(items[i].patern == _patern)
            {
                temp = i;
            }
        }
        return temp;
    }
    public void UseItem(ItemPatern _patern)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
                if (items[i].patern == _patern)
                {
                    items[i].patern.Use(items[i], this);
                    return;
                }
        }
    }
    public void ItemToBackpack()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                items[i].patern.ToBackpack(items[i], this);
            }
        }
    }
    public ItemContainer DropItem()
    {
        if (items[0] != null)
        {
            ItemContainer temp = items[0].patern.Drop(transform.position, items[0]);
            Destroy(items[0].mesh.gameObject);
            items[0] = null;
            return temp;
        }
        return null;
    }
    public ItemContainer DropItem(Item _item)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == _item)
            {
                Item temp = items[i];
                Destroy(items[i].mesh.gameObject);
                items[i] = null;
                transform.GetComponent<WindowManager>().windowPrefab.GetComponent<Window>().Actualize();
                return temp.patern.Drop(transform.position, temp);
            }
        }
        return null;
    }
    public void AddTask(Task _task)
    {
        _task.Take(this);
        tasks.Add(_task);
    }
    public void ChangeRole(Role _role)
    {
        role = _role;
        role.Start();
    }
    public void ChangeRole(string _role)
    {
        switch (_role)
        {
            case "worker":
                {
                    ChangeRole(new Worker(this));
                }break;
            case "miner":
                {
                    ChangeRole(new Miner(this));
                }
                break;
            case "woodcuter":
                {
                    ChangeRole(new Woodcutter(this));
                }
                break;
            case "farmer":
                {
                    ChangeRole(new Farmer(this));
                }
                break;
            default:
                {
                    ChangeRole(new Worker(this));
                }break;
        }
    }
    public void Born()
    {
        firstName = Generator.GetName();
        lastName = "";
        count++;
        ready = true;
        ChangeRole(startRole);
        for (int i = 0; i < startItems.Length; i++)
        {
            AddItem(new Item(startItems[i]));
        }
        transform.GetComponent<WindowManager>().windowPrefab.GetComponent<Window>().Actualize();
    }

    public void NextTask()
    {
        tasks.RemoveAt(0);
    }

    public void SetAnimation(string _name)
    {
        
        string[] names = new string[]
        {
            "idle",
            "walk",
            "chop"            
        };
        GetComponent<Animator>().SetBool("empty",items[0] == null); 
        for(int i=0;i<names.Length;i++)
            if (names[i] == _name)
            {
                GetComponent<Animator>().SetInteger("main", i);
                break;
            }
    }
}
