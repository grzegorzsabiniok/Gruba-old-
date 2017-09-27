using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    static public Dictionary<string, Animation> animations;
    public string currentAnimation;
    private void Start()
    {
        animations[currentAnimation].Play(transform);
    }
    private void Update()
    {
        animations[currentAnimation].Update();
    }

}

