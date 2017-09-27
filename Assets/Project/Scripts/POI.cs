using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public interface POICaler
{
    void Fail(POI poi);
}
public class POI
{
    public string[] tags;
    public Vector3 position;
    public bool oneTime = true;
    public POI[] subpois;
    public POICaler caler;

    public bool HaveTag(string _tag)
    {
        if (tags == null) return false;
        return tags.Contains(_tag);
    }
    public bool Fit(Unit _unit, Type _type, Dictionary<string, object> _parameters)
    {
        if (_type == null)
        {
            return true;
        }
        else
        {
            if (_type == this.GetType())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public virtual bool Match(Unit _unit, Type _type, Dictionary<string, object> _parameters)
    {
        return Fit(_unit, _type, _parameters);
    }
    public virtual void Fail()
    {
        if (caler != null)
        {
            caler.Fail(this);
        }
    }
}
public class POIWork : POI
{
    ItemPatern item;
    Task task;
    public POIWork(ItemPatern _item, Task _task)
    {
        task = _task;
        item = _item;
    }
    public POIWork()
    {
        item = null;
    }
    public override bool Match(Unit _unit, Type _type, Dictionary<string, object> _parameters)
    {
        //task.Take(_unit);
        if (_type != this.GetType()) return false;

        if (_unit.CheckItem(item) != -1 || item == null)
        {
            MonoBehaviour.print("dodaje taska");
            task.poi = this;
            _unit.AddTask(task);
            return true;
        }
        return false;
    }
}
public class POIItem : POI
{
    public ItemContainer item;
    public POIItem(ItemContainer _item)
    {
        item = _item;
    }
    public override bool Match(Unit _unit, Type _type, Dictionary<string, object> _parameters)
    {

        if (_type == this.GetType() || _type == null)
        {
            if (_parameters.ContainsKey("tag"))
            {
                return item.item.HaveTag((string)_parameters["tag"]);
            }
            if (_parameters.ContainsKey("item"))
            {
                if (((ItemPatern)(_parameters["item"])) == item.item.patern)
                {
                    return true;
                }
            }
        }
        return false;

    }
}
public class POIStockpile : POI
{
    public Stockpile stockpile;
    public POIStockpile(Stockpile _stockpile)
    {
        stockpile = _stockpile;
        oneTime = false;
    }
    public override bool Match(Unit _unit, Type _type, Dictionary<string, object> _paramters)
    {
        if (this.GetType() == _type)
        {
            if (_paramters == null)
            {
                if (stockpile.slots[position] == null)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            if (stockpile.slots[position].patern == (ItemPatern)_paramters["item"])
            {
                return true;

            }
        }
        return false;
    }
}