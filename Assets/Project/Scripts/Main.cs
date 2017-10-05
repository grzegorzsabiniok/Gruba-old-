using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main main;
    [Header("Map Settings")]
    public Vector3 chunkSize = new Vector3(16, 64, 16);
    public int mapSeed = 0;
    public Vector3 mapSize;
    public float mapAmplitude = 50f;
    public int mapCubature;
    public int mapHeight;
    public int mapLevelsCount;
    public int mapLevelHeight;
    public int waterLevel;
    public int peakLevel;
    public int peakCubature;
    public int peakHeight;
    public float treeDensity;
    public int rockCount;
    public Dictionary<Vector3, Chunk> chunks = new Dictionary<Vector3, Chunk>();
    [Header("Game Settings")]
    public float unitSpeed;
    public float gameSpeed;
    [Header("Viev Settings")]
    public int topRender = 32;
    [Header("Other Settings")]
    public float SecondTryTime;
    public Material normalPalete, transparentPalete;


    /// <summary>
    int[,,] map;
    //0-air
    //-1 collider
    //-2 - ladder
    //-3 - stockpile
    //-100 - interest point
    /// </summary>
    public List<POI> poi = new List<POI>();
    //public static Dictionary<string, ItemPatern> itemIndex = new Dictionary<string, ItemPatern>();
    public List<string> namesList = new List<string>();
    public void SetSpeed(float speed)
    {
        gameSpeed = speed;
        if (gameSpeed < 0) gameSpeed = 0;
        if (gameSpeed > 10) gameSpeed = 10;

        Animator[] temp = FindObjectsOfType<Animator>();
        foreach (Animator i in temp)
        {
            i.speed = gameSpeed;
        }
    }
    void Awake()
    {
        Stockpile.stockpileIndex = new List<Stockpile>();
        Stockpile.itemIndex = new Dictionary<ItemPatern, int>();
        topRender = (int)(chunkSize.y * mapSize.y);
        mapSeed = PlayerPrefs.GetInt("mapSeed");
        main = this;
        map = new int[(int)(chunkSize.x * mapSize.x), (int)(chunkSize.y * mapSize.y), (int)(chunkSize.z * mapSize.z)];
        Generate();
        for (int i = 0; i < 10; i++)
            print(Generator.GetName());
    }
    public void Generate()
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {

            for (int z = 0; z < map.GetLength(2); z++)
            {
                int t = 0;
                int temp = GetStoneHeight(x, z);
                for (int i = 0; i < temp; i++)
                {
                    map[x, i, z] = (int)Chunk.Material.stone;
                    t = i;
                }

                if (t < waterLevel)
                {
                    map[x, t - 1, z] = (int)Chunk.Material.sand;
                    for (int i = t; i < waterLevel; i++)
                    {
                        map[x, i, z] = (int)Chunk.Material.water;
                    }
                }
                else
                {
                    if (t < peakLevel)
                        map[x, t, z] = (int)Chunk.Material.grass;
                }

            }
        }
    }
    public Vector3 GetMapSize()
    {
        return new Vector3(chunkSize.x * mapSize.x, chunkSize.y * mapSize.y, chunkSize.z * mapSize.z);
    }
    public Chunk[] GetAllChunks()
    {
        Chunk[] temp = new Chunk[chunks.Count];
        chunks.Values.CopyTo(temp, 0);
        return temp;
    }
    public Chunk GetChunk(Vector3 _position)
    {
        Vector3 temp = new Vector3((int)(_position.x) / (int)(chunkSize.x), (int)(_position.y) / (int)(chunkSize.y), (int)(_position.z) / (int)(chunkSize.z));
        if (chunks.ContainsKey(temp))
            return chunks[temp];
        else return null;
    }
    public int GetBlock(int x, int y, int z)
    {
        if (x < 0 || y < 0 || z < 0 || x > GetMapSize().x - 1 || y > GetMapSize().y - 1 || z > GetMapSize().z - 1) return -10;
        return map[x, y, z];
    }
    public int GetBlock(Vector3 _position)
    {
        return GetBlock((int)Mathf.Floor(_position.x), (int)Mathf.Floor(_position.y), (int)Mathf.Floor(_position.z));
    }
    public void SetBlock(Vector3 _position, int _block)
    {
        // _position = Normalize(_position);
        map[(int)_position.x, (int)_position.y, (int)_position.z] = _block;
        if (_block > -1)
        {
            GetChunk(_position).Actualize();
        }
    }
    public int GetTreeRange()
    {
        return (int)(chunkSize.x * mapSize.x - 10);
    }
    public bool GetTreePosition(int _x, int _y)
    {

        if (Mathf.PerlinNoise(_x / 15f + mapSeed + 666, _y / 15f + mapSeed + 666) > treeDensity && GetStoneHeight(_x, _y) > waterLevel && GetStoneHeight(_x, _y) < peakLevel)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public int GetStoneHeight(int _x, int _y)
    {
        int temp = (int)(Mathf.PerlinNoise((_x / mapCubature) * mapCubature / mapAmplitude + mapSeed, (_y / mapCubature) * mapCubature / mapAmplitude + mapSeed) * mapLevelsCount) * mapLevelHeight + mapHeight;
        if (temp >= peakLevel)
        {
            int temp2 = temp - peakLevel;
            temp -= temp2;

            float peak = ((float)peakLevel) / ((mapLevelsCount + 5) * mapLevelHeight + mapHeight);
            temp2 = (int)((Mathf.PerlinNoise((_x / (peakCubature)) * (peakCubature) / mapAmplitude + mapSeed, (_y / (peakCubature)) * (peakCubature) / mapAmplitude + mapSeed) - peak) * mapLevelsCount * peakHeight);
            temp += temp2;
        }
        return temp;
    }
    public int GetPeakHeight(int _x, int _y)
    {
        return (int)(Mathf.PerlinNoise((_x / mapCubature) * mapCubature / mapAmplitude + mapSeed, (_y / mapCubature) * mapCubature / mapAmplitude + mapSeed));
    }
    public static Vector3 Normalize(Vector3 _vec)
    {
        return new Vector3(Mathf.Round(_vec.x), Mathf.Round(_vec.y), Mathf.Round(_vec.z));
    }
}
