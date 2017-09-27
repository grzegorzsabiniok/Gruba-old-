using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEditor : MonoBehaviour
{
    [SerializeField]
    List<string> frames = new List<string>();
    [SerializeField]
    Transform target;
    [SerializeField]
    [TextArea(10, 10)]
    string result;
    string name;
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 30), "add frame")) AddFrame();
        if (GUI.Button(new Rect(0, 30, 100, 30), "generate")) Generate();

    }
    void Generate()
    {
        string parts = "";
        foreach (Transform i in target.GetComponentsInChildren<Transform>())
        {
            if (target.gameObject != i.gameObject)
                parts += i.name + ", ";
        }
        parts = parts.Remove(parts.Length - 2);
        result = "name: " + name + "\n" +
            "\tparts: " + parts + "\n";
        foreach (string frame in frames)
        {
            result += "\tframe: " + frame + "\n";
        }

    }
    void AddFrame()
    {
        string frame = "1,";
        foreach (Transform i in target.GetComponentsInChildren<Transform>())
        {
            if (target.gameObject != i.gameObject)
                frame += i.localPosition.x + "/" + i.localPosition.y + "/" + i.localPosition.z + "|" + i.localEulerAngles.x + "/" + i.localEulerAngles.y + "/" + i.localEulerAngles.z + ", ";
        }
        frame = frame.Remove(frame.Length - 2);
        frames.Add(frame);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
