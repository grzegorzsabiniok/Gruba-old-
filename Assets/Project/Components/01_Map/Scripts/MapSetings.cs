using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapSettings", menuName = "Map/Settings", order = 1)]
public class MapSetings : ScriptableObject
{
    public Vector3 chunkSize = new Vector3(16, 64, 16);
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
}
