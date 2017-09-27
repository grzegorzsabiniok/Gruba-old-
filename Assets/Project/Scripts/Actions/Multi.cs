using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi : Task {
    public Task[] tasks;
    int pointer = 0;
    public Multi(Task[] _tasks)
    {
        tasks = _tasks;
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].parent = this;
        }
        name = "multi";
    }
    public override void Update()
    {
        tasks[pointer].Behaviour();
    }
    public void Finished()
    {       
        pointer++;
        if(pointer == tasks.Length)
        {
            Success();
            return;
        }
    }
    public override void Take(Unit _owner)
    {
        for(int i = 0; i < tasks.Length; i++)
        {
            tasks[i].Take(_owner);
        }
        base.Take(_owner);
    }
}
