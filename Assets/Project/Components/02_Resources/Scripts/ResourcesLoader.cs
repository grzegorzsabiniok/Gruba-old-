using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ResourcesLoader
{
    [SerializeField]
    private string path;
    public ResourcesLoader(string path)
    {
        this.path = Directory.GetCurrentDirectory() + path;
    }
    public Dictionary<string, T> Load<T>(string name) where T : ResourceObject
    {
        Dictionary<string, T> colection = new Dictionary<string, T>();
        string[] files = Directory.GetFiles(path + name);
        foreach (string file in files)
        {
            StreamReader reader = new StreamReader(file);
            JsonSerializer serializer = new JsonSerializer();
            foreach (var variable in (List<T>)serializer.Deserialize(reader, typeof(List<T>)))
            {
                if (colection.ContainsKey(variable.GetName()))
                {
                    colection[variable.GetName()] = variable;
                }
                else
                {
                    colection.Add(variable.GetName(), variable);
                }
            }
            reader.Close();
        }
        return colection;
    }

}
