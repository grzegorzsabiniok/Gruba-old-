using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldWindow : Window {
    public Text desc;
    public Image icon;
    public Sprite empty;
    Field owner;
    void Start()
    {
        //title.text = owner.item.patern.name;
        if (owner.plant != "") {
            icon.sprite = Plant.GetPlant(owner.plant).revard.icon;
            desc.text = owner.plant;
        }
        else {
            icon.sprite = empty;
            desc.text = "empty";
        }
        
    }
    public override void Actualize()
    {
        if (owner.plant != "")
        {
            icon.sprite = Plant.GetPlant(owner.plant).revard.icon;
            desc.text = owner.plant;
        }
        else
        {
            icon.sprite = empty;
            desc.text = "empty";
        }
    }
    public void Clear()
    {
        owner.ChangePlant(null);
    }
    public override void Connect(Transform _owner)
    {
        owner = _owner.GetComponent<Field>();
    }
}
