using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {

    public Transform windowPrefab;
    public WindowManager parent;
    public float bornTime;
    private void Start()
    {
        if (parent == null)
        {
            Transform temp = Instantiate(windowPrefab);
            temp.GetComponent<Window>().Connect(transform);
            temp.SetParent(GameObject.Find("Canvas").transform);
            temp.localPosition = new Vector2(0, 0);
            windowPrefab = temp;
            windowPrefab.gameObject.SetActive(false);
        }
    }
    private void OnMouseDown()
    {
        print(Time.time + "|" + bornTime);
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Act();
        }
    }
    public void Actualize()
    {
        if (parent == null)
        {
            windowPrefab.GetComponent<Window>().Actualize();
        }
        else
        {
            parent.Actualize();
        }
    }
    public void Act()
    {
        if (Time.time > bornTime)
        {
            if (parent != null)
            {
                parent.Act();
            }
            else
            {
                windowPrefab.gameObject.SetActive(true);
                windowPrefab.SetSiblingIndex(windowPrefab.root.childCount);
            }
        }
    }
}
