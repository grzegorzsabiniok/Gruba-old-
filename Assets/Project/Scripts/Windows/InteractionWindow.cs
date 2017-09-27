using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class InteractionWindow : Window
{
    Structure owner;
    void Start()
    {
        title.text = owner.name;
    }
    public override void Connect(Transform _owner)
    {
        owner = _owner.GetComponent<Structure>();
        Actualize();
    }
    public void Use()
    {
        owner.Interact();
    }
}
