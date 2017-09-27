using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class UnitWindow : Window {
    Unit owner;
    public Dropdown rolePicker;
    public Sprite empty;
    public Text desc;
    private void Update()
    {
        if (owner != null)
        {
            string temp = owner.firstName + " " + owner.lastName +"\nNeeds:\n";
            foreach(Need need in owner.needs.Values)
            {
                temp += "\t-" + need.name + " " + Mathf.Floor(need.level) + "\n";
            }
            if (owner.actualNeed != null)
                temp += "actual " + owner.actualNeed.name;
            desc.text = temp;
        }
    }
    public override void Connect(Transform _owner)
    {
        owner = _owner.GetComponent<Unit>();
        Actualize();
    }
    public void ChangeRole()
    {
        int _role = rolePicker.value;
        Role[] roles = new Role[] { new Worker(owner), new Miner(owner), new Woodcutter(owner), new Farmer(owner) };
        owner.ChangeRole(roles[_role]);
    }
    public void FirstPerson()
    {
        Camera[] temp = FindObjectsOfType<Camera>();
        foreach(Camera i in temp)
        {
            i.enabled = false;
        }
        owner.eye.enabled = true;
    }
    public override void Actualize()
    {
        if (owner != null)
        {
            for (int i = 0; i < owner.items.Length; i++)
            {
                if (owner.items[i] == null)
                {
                    transform.Find("Main/Item" + i).GetComponent<Image>().sprite = empty;
                }
                else
                {
                    transform.Find("Main/Item" + i).GetComponent<Image>().sprite = owner.items[i].patern.icon;
                }
            }
            //desc.text = owner.firstName + " " + owner.lastName + "\n";
        }
    }
}
