using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class Resources : MonoBehaviour
{
    public static Resources singleton;
    [SerializeField]
    private Material[] materialsPrefabs;

    private Dictionary<string, Material> materials = new Dictionary<string, Material>();
    private Dictionary<string, BlockMaterial> blocksMaterials = new Dictionary<string, BlockMaterial>();

    private void Awake()
    {
        singleton = this;
        foreach (Material material in materialsPrefabs)
        {
            materials.Add(material.name, material);
        }
    }
    private void Start()
    {
        ResourcesLoader loader = new ResourcesLoader("\\Data\\");
        blocksMaterials = loader.Load<BlockMaterial>("Blocks\\");
        print(blocksMaterials["air"].GetName());
    }
    public static Material GetMaterial(string name)
    {
        if (singleton.materials.ContainsKey(name))
        {
            return singleton.materials[name];
        }
        else
        {
            return null;
        }
    }
}
