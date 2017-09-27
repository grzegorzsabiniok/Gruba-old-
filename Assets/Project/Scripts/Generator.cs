using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Generator{
    
    public static string GetName()
    {
        string[] syllables = new string[] { "mon", "fay", "shi", "zag", "blarg", "rash", "izen" };

        string name = "";
        int length = Random.Range(2, 4);
        for(int i = 0; i < length; i++)
        {
            name += syllables[Random.Range(0, syllables.Length)];
        }
        return name;
    }

}
