using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DragAndDrop : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    Vector3 mouse;
    public void OnDrag(PointerEventData eventData)
    {
        transform.parent.position += Input.mousePosition - mouse;
    }
    void Update()
    {
        mouse = Input.mousePosition;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.parent.SetSiblingIndex(transform.root.childCount);
    }
}
