using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPatern {
    public string name;
    public int size;
    public string[] tags;
    public Sprite icon;
    public string target;
    public Mesh model;
    public Transform containerPrefab, itemPrefab;
    public string[] resources;
    public Dictionary<string, string> properties;
    //onGround
    public Vector3 positionOnGround;
    public Vector3 rotationOnGround;
    public Vector3 scaleOnGround;
    //inHand
    public Vector3 positionInHand;
    public Vector3 rotationInHand;
    public Vector3 scaleInHand;
    public string InHand;
    //inBackpack
    public Vector3 positionInBackpack;
    public Vector3 rotationInBackpack;
    public Vector3 scaleInBackpack;
    public string InBackpack;
/*
    private void Start()
    {
        print(
            "@name: "+name+"\n"+
            "\tsize: " + size + "\n" +
            "\ttags: \n" +
            "\ticon: \n" +
            "\tmodel: \n" +
            "\tresources: \n" +
            "\tonGround: " + positionOnGround + " " + rotationOnGround + " " +scaleOnGround.x + "\n" +
            "\tinHand: " + InHand + " " +positionInHand + " " +rotationInHand + " " +scaleInHand.x + "\n" +
            "\tinBackpack: " + InBackpack + " "+positionInBackpack + " "+rotationInBackpack + " "+ scaleInBackpack.x+ "\n"
            );
    }
*/
    public ItemPatern(Transform _container,Transform _item)
    {
        itemPrefab = _item;
        containerPrefab = _container;
    }
    public ItemContainer Drop(Vector3 _position,Item _item)
    {
        Transform temp = Transform.Instantiate(containerPrefab);
        temp.Find("default").GetComponent<MeshFilter>().sharedMesh =model;
        temp.position = _position;
        temp.Find("default").localPosition = positionOnGround;
        temp.Find("default").eulerAngles = rotationOnGround;
        temp.Find("default").localScale = scaleOnGround;
        temp.GetComponent<ItemContainer>().item = _item;
        _item.container = temp;
        return temp.GetComponent<ItemContainer>();
    }
    public void Put(Item _item,Unit _unit)
    {
        Transform temp = Transform.Instantiate(itemPrefab);
        temp.Find("default").GetComponent<MeshFilter>().sharedMesh = model;
        temp.SetParent(_unit.transform.Find(InBackpack));
        temp.localPosition = positionInBackpack;
        temp.localEulerAngles = rotationInBackpack;
        temp.localScale = scaleInBackpack;
        _item.mesh = temp;

    }
    public void Use(Item _item, Unit _unit)
    {
        Transform temp = _item.mesh;
        temp.SetParent(_unit.transform.Find(InHand));
        temp.localPosition = positionInHand;
        temp.localEulerAngles = rotationInHand;
        temp.localScale = scaleInHand;

    }
    public void ToBackpack(Item _item, Unit _unit)
    {
        Transform temp = _item.mesh;
        temp.SetParent(_unit.transform.Find(InBackpack));
        temp.localPosition = positionInBackpack;
        temp.localEulerAngles = rotationInBackpack;
        temp.localScale = scaleInBackpack;
    }
}
