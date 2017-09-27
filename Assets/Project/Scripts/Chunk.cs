using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Chunk : MonoBehaviour
{
    public bool sliced = false,render = false;
    public int textureCount;
    struct Level
    {
        public int terrainCount, waterCount;
        public Mesh terrain,water;
        public Level(int _terrainCount,int _waterCount,Mesh _terrain,Mesh _water)
        {
            terrainCount = _terrainCount;
            waterCount = _waterCount;
            terrain = _terrain;
            water = _water;
        }
    }
    class TempMesh
    {
        public List<Vector3> vertex;
        public List<Vector2> uv;
        public List<int> face;
        public TempMesh()
        {
            vertex = new List<Vector3>();
            uv = new List<Vector2>();
            face = new List<int>();
        }
    }
    public enum Material
    {
        water=1,
        grass=2,
        stone=3,
        dirt=4,
        sand=5,
        clay=6
    }
    public Vector3 chunkNumber;
    public Transform waterPrefab,coverPrefab,waterCoverPrefab;
    List<Vector3> vertex = new List<Vector3>();
    List<Color> color = new List<Color>();
    List<Vector2> uv = new List<Vector2>();
    List<int> face = new List<int>();
    Level[] levels;
    Mesh allTerrain,allWater;
    int x;
    void Update()
    {
        x++;
        if(render || sliced)
        if (x % (Main.main.mapSize.x * Main.main.mapSize.x) == (int)(chunkNumber.x * Main.main.mapSize.x + chunkNumber.z))
        {
            if (render)
            {
                Render();
                render = false;
            }
            if (sliced)
            {

                sliced = false;
                Slice();
            }
        }
    }
    public void Actualize()
    {
        render = true;
        sliced = true;
    }
    void Awake()
    {
        levels = new Level[(int)(Main.main.chunkSize.y)];
    }
    void Start()
    {
        Render();
    }
    public void Render()
    {
        TempMesh terrain = new TempMesh();
        TempMesh water = new TempMesh();
        for (int y = (int)(Main.main.chunkSize.y*chunkNumber.y); y < (int)(Main.main.chunkSize.y * (chunkNumber.y+1)); y++)
        {
            TempMesh terrainCover = new TempMesh();
            TempMesh waterCover = new TempMesh();
            for (int x = (int)(Main.main.chunkSize.x * chunkNumber.x); x < (int)(Main.main.chunkSize.x * (chunkNumber.x + 1)); x++)
            {
                for (int z = (int)(Main.main.chunkSize.z * chunkNumber.z); z < (int)(Main.main.chunkSize.z * (chunkNumber.z + 1)); z++)
                {
                    if (Main.main.GetBlock(x, y, z) > 0)
                    {
                        if (Main.main.GetBlock(x, y, z) != (int)Material.water)
                        {
                            if (Avaible(x, y + 1, z))
                            {
                                Face(terrain,Main.main.GetBlock(x, y, z), 0, new Vector3(x, y + 1, z), new Vector3(x + 1, y + 1, z), new Vector3(x, y + 1, z + 1), new Vector3(x + 1, y + 1, z + 1));

                            }
                            else
                            {
                                Face(terrainCover,Main.main.GetBlock(x, y, z), 0, new Vector3(x, y + 1, z), new Vector3(x + 1, y + 1, z), new Vector3(x, y + 1, z + 1), new Vector3(x + 1, y + 1, z + 1));

                            }
                        }
                        else
                        {
                            if (Avaible(x, y + 1, z,0))
                            {
                                Face(water, Main.main.GetBlock(x, y, z), 0, new Vector3(x, y + 1, z), new Vector3(x + 1, y + 1, z), new Vector3(x, y + 1, z + 1), new Vector3(x + 1, y + 1, z + 1));

                            }
                            else
                            {
                                Face(waterCover, Main.main.GetBlock(x, y, z), 0, new Vector3(x, y + 1, z), new Vector3(x + 1, y + 1, z), new Vector3(x, y + 1, z + 1), new Vector3(x + 1, y + 1, z + 1));

                            }
                        }
                        if (Main.main.GetBlock(x, y, z) != (int)Material.water)
                        {
                            if (Avaible(x, y - 1, z))
                                Face(terrain, Main.main.GetBlock(x, y, z), 1, new Vector3(x + 1, y, z + 1), new Vector3(x + 1, y, z), new Vector3(x, y, z + 1), new Vector3(x, y, z));

                            if (Avaible(x + 1, y, z))
                                Face(terrain, Main.main.GetBlock(x, y, z), 2, new Vector3(x + 1, y, z), new Vector3(x + 1, y, z + 1), new Vector3(x + 1, y + 1, z), new Vector3(x + 1, y + 1, z + 1));

                            if (Avaible(x - 1, y, z))
                                Face(terrain, Main.main.GetBlock(x, y, z), 2, new Vector3(x, y, z + 1), new Vector3(x, y, z), new Vector3(x, y + 1, z + 1), new Vector3(x, y + 1, z));

                            if (Avaible(x, y, z - 1))
                                Face(terrain, Main.main.GetBlock(x, y, z), 2, new Vector3(x, y, z), new Vector3(x + 1, y, z), new Vector3(x, y + 1, z), new Vector3(x + 1, y + 1, z));

                            if (Avaible(x, y, z + 1))
                                Face(terrain, Main.main.GetBlock(x, y, z), 2, new Vector3(x + 1, y, z + 1), new Vector3(x, y, z + 1), new Vector3(x + 1, y + 1, z + 1), new Vector3(x, y + 1, z + 1));
                        }
                    }
                }  
            }
            Mesh temp = new Mesh();
            temp.vertices = terrainCover.vertex.ToArray() ;
            temp.uv = terrainCover.uv.ToArray();
            temp.triangles = terrainCover.face.ToArray();
            Mesh temp2 = new Mesh();
            temp2.vertices = waterCover.vertex.ToArray();
            temp2.uv = waterCover.uv.ToArray();
            temp2.triangles = waterCover.face.ToArray();
            levels[y-(int)(chunkNumber.y*Main.main.chunkSize.y)] = new Level(terrain.face.Count,water.face.Count,temp,temp2);
        }
        allTerrain = new Mesh();
        allTerrain.vertices = terrain.vertex.ToArray();
        allTerrain.uv = terrain.uv.ToArray();
        allTerrain.triangles = terrain.face.ToArray();
        allTerrain.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = allTerrain;
        GetComponent<MeshCollider>().sharedMesh = allTerrain;

        allWater = new Mesh();
        allWater.vertices = water.vertex.ToArray();
        allWater.uv = water.uv.ToArray();
        allWater.triangles = water.face.ToArray();
        allWater.RecalculateNormals();
        waterPrefab.GetComponent<MeshFilter>().mesh = allWater;
    }
    public void Slice()
    {
        int t = Main.main.topRender;
        if (t >= (chunkNumber.y + 1) * (Main.main.chunkSize.y))
        {
            t = (int)Main.main.chunkSize.y - 1;
            coverPrefab.GetComponent<MeshFilter>().mesh = null;
            coverPrefab.GetComponent<MeshCollider>().sharedMesh = null;
        }
        else
        {
            if (t < (chunkNumber.y) * (Main.main.chunkSize.y))
            {
                GetComponent<MeshFilter>().mesh.triangles = null;
                waterPrefab.GetComponent<MeshFilter>().mesh.triangles =null;
                coverPrefab.GetComponent<MeshFilter>().mesh = null;
                coverPrefab.GetComponent<MeshCollider>().sharedMesh = null;
                waterCoverPrefab.GetComponent<MeshFilter>().mesh = null;
                GetComponent<MeshCollider>().sharedMesh = null;
                return;
            }
            else
            {
                t = t % (int)Main.main.chunkSize.y;
            }
            coverPrefab.GetComponent<MeshFilter>().mesh = levels[t].terrain;
            coverPrefab.GetComponent<MeshCollider>().sharedMesh = levels[t].terrain;
        }
        GetComponent<MeshFilter>().mesh.triangles = allTerrain.triangles.Take(levels[t].terrainCount).ToArray();
        waterPrefab.GetComponent<MeshFilter>().mesh.triangles = allWater.triangles.Take(levels[t].waterCount).ToArray();
        waterCoverPrefab.GetComponent<MeshFilter>().mesh = levels[t].water;
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
    }
    void Optimize()
    {

    }
    bool Avaible(int _x, int _y, int _z)
    {
        return Avaible(_x, _y, _z, 1);
    }
    bool Avaible(int _x, int _y, int _z,int _mod)
    {
        if (_x > Main.main.GetMapSize().x - 1 || _x < 0 || _y > Main.main.GetMapSize().y - 1 || _y < 0 || _z > Main.main.GetMapSize().z - 1 || _z < 0) return false;
        else
        {
            if (Main.main.GetBlock(_x, _y, _z) <= _mod)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    void Face(TempMesh mesh,int material, int side, Vector3 q1, Vector3 q2, Vector3 q3, Vector3 q4)
    {
        int a = mesh.vertex.Count;
        mesh.vertex.Add(q1);
        mesh.vertex.Add(q2);
        mesh.vertex.Add(q3);
        mesh.vertex.Add(q4);

        mesh.face.Add(a);
        mesh.face.Add(a + 2);
        mesh.face.Add(a + 1);
        mesh.face.Add(a + 2);
        mesh.face.Add(a + 3);
        mesh.face.Add(a + 1);

        mesh.uv.Add(new Vector2((float)(material-1)/textureCount+0.1f,0));
        mesh.uv.Add(new Vector2((float)(material) / textureCount-0.1f, 0));
        mesh.uv.Add(new Vector2((float)(material - 1)/ textureCount + 0.1f, 1));
        mesh.uv.Add(new Vector2((float)(material)  / textureCount - 0.1f, 1));
        /*
        mesh.uv.Add(Main.main.GetMapTextures(material,0));
        mesh.uv.Add(Main.main.GetMapTextures(material, 1));
        mesh.uv.Add(Main.main.GetMapTextures(material, 2));
        mesh.uv.Add(Main.main.GetMapTextures(material, 3));
        */
    }
}
