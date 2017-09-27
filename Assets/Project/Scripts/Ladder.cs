using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Structure
{

    public override void GenerateColiders()
    {
        ModifyColiders(-2);
        print("dziala");
    }
}