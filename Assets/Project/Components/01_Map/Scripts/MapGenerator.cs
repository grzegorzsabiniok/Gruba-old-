using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private Block[] listOfBlocks;
    private Dictionary<string, Block> blocks = new Dictionary<string, Block>();
    [SerializeField]
    private MapSetings settings;
    private Block[,,] map;
    private void Awake()
    {
        for (int i = 0; i < listOfBlocks.Length; i++)
        {
            //blocks.Add(listOfBlocks[i].name, listOfBlocks[i]);
        }
    }
    public Block[,,] GenerateMap()
    {
        map = new Block[settings.MapSize.x, settings.MapSize.y, settings.MapSize.z];
        for (int x = 0; x < settings.MapSize.x; x++)
        {
            for (int z = 0; z < settings.MapSize.z; z++)
            {
                GetHeight(x, z);
            }
        }
        return map;
    }
    private void GetHeight(int x, int z)
    {
        for (int y = 0; y < settings.MapSize.y; y++)
        {
            if (y < 10)
            {
                SetBlock(x, y, z, "stone");
            }
            else
            {
                SetBlock(x, y, z, "air");
            }
        }
    }
    private void SetBlock(int x, int y, int z, string name)
    {
        map[x, y, z] = blocks[name];
    }

}
