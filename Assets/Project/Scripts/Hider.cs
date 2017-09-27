using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : MonoBehaviour {
    MeshRenderer[] render;
    int memory;
    float memory2;
    void Start()
    {
        render = gameObject.GetComponentsInChildren<MeshRenderer>();
    }
	void Update () {
        if (memory != Main.main.topRender || transform.position.y!=memory2)
        {
            memory = Main.main.topRender;
            memory2 = transform.position.y;
            if (transform.position.y > Main.main.topRender+1)
            {
                for(int i = 0; i < render.Length; i++)
                {
                    render[i].enabled = false;
                }
            }
            else
            {
                for (int i = 0; i < render.Length; i++)
                {
                    render[i].enabled = true;
                }
            }
        }
    }
}
