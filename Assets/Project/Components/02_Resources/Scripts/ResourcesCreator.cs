using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class ResourcesCreator : MonoBehaviour
{
    [SerializeField]
    private Block block;
    [SerializeField]
    private string output;
    private void Start()
    {
        output = JsonConvert.SerializeObject(block);
    }
}
