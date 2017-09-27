using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Window : MonoBehaviour{

    public Text title;

    public void Exit()
    {
        gameObject.SetActive(false);
    }
    public void Close(Transform _target)
    {
        _target.gameObject.SetActive(false);
    }
    public virtual void Connect(Transform _owner) { }
    public virtual void Actualize() { }
}

