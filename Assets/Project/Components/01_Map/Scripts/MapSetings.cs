using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapSettings", menuName = "Map/Settings", order = 1)]
public class MapSetings : ScriptableObject
{
    [SerializeField]
    private Vector3Int chunkSize, chunkCount;
    [SerializeField]
    private float mapAmplitude, treeDensity;
    [SerializeField]
    private int mapCubature, mapHeight, mapLevelsCount, mapLevelHeight, waterLevel, peakLevel, peakCubature, peakHeight;
    public Vector3Int MapSize
    {
        get
        {
            return new Vector3Int(
                chunkCount.x * chunkSize.x,
                chunkCount.y * chunkSize.y,
                chunkCount.z * chunkSize.z
                );
        }
    }
}
