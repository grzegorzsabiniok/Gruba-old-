using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager singleton;
    [SerializeField]
    private MapSetings settings;
    [SerializeField]
    private int seed;
    private Block[,,] mapGrid;
    [SerializeField]
    private Block[] listOfBlocks;
    private Dictionary<string, Block> blocks = new Dictionary<string, Block>();
    private Chunk[,,] chunks;
    private void Awake()
    {
        singleton = this;
        for (int i = 0; i < listOfBlocks.Length; i++)
        {
            blocks.Add(listOfBlocks[i].name, listOfBlocks[i]);
        }
    }

    public void GenerateMapGrid()
    {
        mapGrid = new Block[settings.MapSize.x, settings.MapSize.y, settings.MapSize.z];
        for (int x = 0; x < settings.MapSize.x; x++)
        {
            for (int z = 0; z < settings.MapSize.z; z++)
            {
                GetHeight(x, z);
            }
        }
    }
    public void GetHeight(int x, int z)
    {
        for (int y = 0; y < settings.MapSize.y; y++)
        {
            if (y < 10)
            {
                mapGrid[x, y, z] = blocks["stone"];
            }
            else
            {
                mapGrid[x, y, z] = blocks["air"];
            }
        }
    }
}
