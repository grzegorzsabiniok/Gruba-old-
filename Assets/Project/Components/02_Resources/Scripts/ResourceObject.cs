using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class ResourceObject
{
    [JsonProperty]
    protected string name;
    public string GetName()
    {
        return name;
    }
}
