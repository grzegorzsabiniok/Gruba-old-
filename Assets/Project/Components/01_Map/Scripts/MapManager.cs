using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager singleton;
    [SerializeField]
    private MapSetings settings;
    private MapBlock[,,] mapGrid;

    private void Awake()
    {
        singleton = this;
    }

    public void GenerateMapGrid()
    {
        //mapGrid = new MapBlock[settings.
    }
}
