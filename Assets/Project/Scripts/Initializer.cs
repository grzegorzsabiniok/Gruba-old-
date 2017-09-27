using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour {
    public Transform chunkPrefab;
    public Transform chunkContainerPrefab;
    public Transform treePrefab;
    public Transform treeContainerPrefab;
    public Transform rockPrefab;
    int treeCount = 0;
    Vector2 mapSize = Vector2.zero;
    int trianglesCount;
	void Start () {

        //Map Generation
        for (int y = 0; y < Main.main.mapSize.y; y++)
        {
            for (int z = 0; z < Main.main.mapSize.z; z++)
            {
                for (int x = 0; x < Main.main.mapSize.x; x++)
                {
                    Transform temp = Transform.Instantiate(chunkPrefab);
                    temp.position = new Vector3(-0.5f, 0, -0.5f);
                    temp.GetComponent<Chunk>().chunkNumber = new Vector3(x, y, z);
                    //temp.GetComponent<Chunk>().Generate();
                    temp.SetParent(chunkContainerPrefab);
                    Main.main.chunks.Add(new Vector3(x, y, z), temp.GetComponent<Chunk>());
                }
            }
        }
        mapSize = new Vector2(Main.main.mapSize.x * Main.main.chunkSize.x, Main.main.mapSize.z * Main.main.chunkSize.z);
        //Trees
        for (int y = 10; y <Main.main.GetTreeRange(); y+=5)
        {
            for (int x = 10; x < Main.main.GetTreeRange(); x+=5)
            {
                if (Main.main.GetTreePosition(x, y))
                {
                    Transform temp = Transform.Instantiate(treePrefab);
                    temp.position = new Vector3(x, Main.main.GetStoneHeight(x, y),y);
                    if (!temp.GetComponent<Structure>().CanPlace())
                    {
                        Destroy(temp.gameObject);
                    }
                    else {
                        temp.GetComponent<Structure>().GenerateColiders();
                        treeCount++;
                        temp.parent = treeContainerPrefab;
                    }
                }
            }
        }
        //rocks
        Random.seed = Main.main.mapSeed + 2137;
        for (int i = 0; i < Main.main.rockCount; i++)
        {
            Transform temp = Transform.Instantiate(rockPrefab);
            
            int x = Random.Range(10, Main.main.GetTreeRange());
            int y = Random.Range(10, Main.main.GetTreeRange());
            temp.position = new Vector3(x, Main.main.GetStoneHeight(x, y), y);
            if (!temp.GetComponent<Structure>().CanPlace())
            {
                Destroy(temp.gameObject);
            }
            else
            {
                temp.GetComponent<Structure>().GenerateColiders();
                temp.parent = treeContainerPrefab;
            }
        }
        print(GetLog());
    }
    public string GetLog()
    {
        return "trees count: " + treeCount + "\nmap size: " + mapSize;
    }

}
