using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Animation
{
    Frame[] frames;
    public float time, speed;
    int targetFrame;
    int currentFrame;
    Transform[] objects;
    string[] objectsNames;
    public string name;
    public Animation(string _name)
    {
        name = _name;
        speed = 1;
    }
    public Animation(Transform[] _objects, Frame[] _frames)
    {
        objects = _objects;
        frames = _frames;
    }
    public void SetObjects(string[] _objects)
    {
        objectsNames = _objects;
    }
    public void SetFrames(Frame[] _frames)
    {
        frames = _frames;
    }
    public void Play(Transform _owner)
    {
        objects = objectsNames.Select(x => _owner.Find(x)).ToArray();
        currentFrame = 0;
        targetFrame = 1;

        time = 0;

    }
    public void Update()
    {
        time += Time.deltaTime * Main.main.gameSpeed * speed;
        if (time > frames[targetFrame].GetTime())
        {
            NextFrame();
        }
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].localPosition = Vector3.Lerp(frames[currentFrame].positions[i], frames[targetFrame].positions[i], time / frames[targetFrame].time);
            objects[i].localRotation = Quaternion.Lerp(frames[currentFrame].rotations[i], frames[targetFrame].rotations[i], time / frames[targetFrame].time);

        }
    }

    void NextFrame()
    {
        currentFrame = targetFrame;
        if (targetFrame + 1 < frames.Length)
        {
            targetFrame = currentFrame + 1;
        }
        else
        {
            targetFrame = 0;

        }
        time = 0;
    }
}
public class Frame
{
    public float time;
    public Vector3[] positions;
    public Quaternion[] rotations;
    public Frame(float _time, Vector3[] _positions, Vector3[] _rotations)
    {
        time = _time;
        positions = _positions;
        rotations = _rotations.Select(x => Quaternion.Euler(x)).ToArray(); ;
    }
    public float GetTime() { return time; }
}

