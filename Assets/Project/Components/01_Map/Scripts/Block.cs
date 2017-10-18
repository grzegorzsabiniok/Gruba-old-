using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block
{
    private BlockMaterial material;
    private float temperature;
    private List<Subject> objectsOnBlock = new List<Subject>();
}
