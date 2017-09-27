using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Willage : MonoBehaviour {
    public static Willage willage;
    public List<Task> tasks = new List<Task>();
    public List<Stockpile> stockpiles = new List<Stockpile>();
    public List<Citizen> citizens = new List<Citizen>();
	// Use this for initialization
	void Awake () {
        willage = this;
	}
	public void RemoveItem(Item _item)
    {
        for(int i = 0; i < stockpiles.Count; i++)
        {
            stockpiles[i].RemoveItem(_item);
        }
    }
	// Update is called once per frame
    public Task GetTask(Vector3 _position)
    {
        return null;
        /*
        print(_position+" "+tasks[0].position);
        Task temp = tasks.FirstOrDefault(x => x.position == _position);
        if (temp != null)
        {
            tasks.Remove(temp);
        }
        return temp;
        */
    }
    public void AddTask(Task _task)
    {
        tasks.Add(_task);
    }
	void Update () {
        /*
		for(int i = 0; i < citizens.Count; i++)
        {
            if(citizens[i].actions.Count == 0)
            {
                for(int q = 0; q < tasks.Count; q++)
                {
                    if(tasks[q].owner == null)
                    {
                        tasks[q].Take(citizens[i]);
                        break;
                    }
                }
            }
        }
        */
	}
}
