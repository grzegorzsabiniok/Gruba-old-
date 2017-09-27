using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task {
    /*
    public Vector3 position;
    public Unit owner;
    int curent;
    public object connector;
    public List<Action> toDo = new List<Action>();
    public Task(Vector3 _position,Action[] actions)
    {
        owner = null;
        position = Main.Normalize(_position);
        toDo = new List<Action>(actions);
        for (int i = 0; i < toDo.Count; i++)
        {
            toDo[i].Take(this);
        }
    }
    public Task(Action[] actions)
    {
        owner = null;
        toDo = new List<Action>(actions);
        for (int i = 0; i < toDo.Count; i++)
        {
            toDo[i].Take(this);
        }
    }
    public bool Act()
    {
        if(curent >= toDo.Count)
        {
            return false;
        }
        if (!toDo[curent].Act())
        {
            curent++;
        }
        return true;
    }
    public void Take(Unit _owner)
    {
        owner = _owner;
        owner.tasks.Add(this);
    }
    public void Take(Unit _owner,Vector3[] path)
    {
        Go temp = new Go(path);
        temp.Take(this);
        toDo.Insert(0, temp);
        Take(_owner);
    }
    */
    bool firstTime = true;
    public Unit owner;
    public Multi parent;
    public POI poi;
    public string name = "something";

    public virtual void Behaviour()
    {
        if (firstTime)
        {
            Start();
            firstTime = false;
        }
        Update();
    }
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void Success()
    {
        if(parent != null)
        {
            parent.Finished();
        }
        else
        {
            owner.NextTask();
        }
    }
    public virtual void Failure()
    {
        if(owner.tasks[0].poi != null)
        {
            owner.tasks[0].poi.Fail();
        }
        owner.SetAnimation("idle");
        owner.NextTask();
    }
    public virtual Task Clone()
    {
        return null;
    }
    public virtual void Take(Unit _owner)
    {
        owner = _owner;
    }


}
