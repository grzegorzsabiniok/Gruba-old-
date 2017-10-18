using UnityEngine;
using Newtonsoft.Json;
using System.Runtime.Serialization;

[System.Serializable]
public class BlockMaterial : ResourceObject
{
    [JsonIgnore]
    private Material textureMaterial;
    [JsonProperty]
    private string textureMaterialName;
    public BlockMaterial(string name, string textureName)
    {
        this.name = name;
        this.textureMaterialName = textureName;
    }
    [OnDeserialized]
    internal void OnDeserialization(StreamingContext context)
    {
        textureMaterial = Resources.GetMaterial(textureMaterialName);
    }

}