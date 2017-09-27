using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemWindow : Window
{
    Item owner;
    public Image icon;
    public Text descryption;
    public Transform buttonPrefab;
    public Transform actionsContainer;
    void Start()
    {
        icon.sprite = owner.patern.icon;
        descryption.text = owner.patern.name;
        if (owner.HaveTag("plant"))
        {
            Transform temp = Instantiate(buttonPrefab);
            temp.SetParent(actionsContainer);
            temp.Find("Text").GetComponent<Text>().text = "plant";
            temp.GetComponent<Button>().onClick.AddListener(delegate { owner.Plant(); });
            temp.GetComponent<Button>().onClick.AddListener(delegate { Exit(); });
            GetComponent<RectTransform>().sizeDelta = new Vector2(155, 105);
        }
        if (owner.HaveTag("structure"))
        {
            Transform temp = Instantiate(buttonPrefab);
            temp.SetParent(actionsContainer);
            temp.Find("Text").GetComponent<Text>().text = "place";
            temp.GetComponent<Button>().onClick.AddListener(delegate { owner.Place(); });
            temp.GetComponent<Button>().onClick.AddListener(delegate { Exit(); });
            GetComponent<RectTransform>().sizeDelta = new Vector2(155, 105);
        }
    }
    //void AddOption(string _name,)
    public override void Connect(Transform _owner)
    {
        owner = _owner.GetComponent<ItemContainer>().item;
    }
}
