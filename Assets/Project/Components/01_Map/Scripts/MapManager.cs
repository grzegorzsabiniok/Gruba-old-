using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager singleton;
    [SerializeField]
    private int seed;
    private Block[,,] mapGrid;
    [SerializeField]
    private MapGenerator generator;
    private Chunk[,,] chunks;
    private void Start()
    {
        GenerateMapGrid();
    }
    public void GenerateMapGrid()
    {
        mapGrid = generator.GenerateMap();
    }

}
