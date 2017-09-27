using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class Loader : MonoBehaviour
{
    public bool animationEditor;
    public Transform containerPrefab, itemPrefab, structurePrefab;
    Dictionary<string, Mesh> meshIndex = new Dictionary<string, Mesh>();
    Dictionary<string, Sprite> iconsIndex = new Dictionary<string, Sprite>();
    Dictionary<string, ItemPatern> itemsIndex = new Dictionary<string, ItemPatern>();
    Dictionary<string, Plant> plantsIndex = new Dictionary<string, Plant>();
    Dictionary<string, Transform> structureIndex = new Dictionary<string, Transform>();
    Dictionary<string, Animation> animationsIndex = new Dictionary<string, Animation>();
    void Awake()
    {
        string[] temps;
        if (!animationEditor)
        {
            //load models
            temps = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Data\\Models\\");
            foreach (string i in temps)
            {
                string temp = i.Replace(Directory.GetCurrentDirectory() + "\\Data\\Models\\", "");
                temp = temp.Remove(temp.Length - 4);
                meshIndex.Add(temp, ObjImporter.ImportFile(i));
            }
            //load icons
            temps = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Data\\Icons\\");
            foreach (string i in temps)
            {
                string temp = i.Replace(Directory.GetCurrentDirectory() + "\\Data\\Icons\\", "");
                temp = temp.Remove(temp.Length - 4).Trim();
                byte[] data = File.ReadAllBytes(i);
                Texture2D texture = new Texture2D(400, 400, TextureFormat.ARGB32, false);
                texture.LoadImage(data);
                texture.name = temp;
                iconsIndex.Add(temp, Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f));
            }
            //load itempaterns

            temps = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Data\\Items\\");
            foreach (string i in temps)
            {
                LoadItem(i);
            }
            itemsIndex.Remove("x");
            Item.index = itemsIndex;

            //load plants

            temps = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Data\\Plants\\");
            foreach (string i in temps)
            {
                LoadPlant(i);
            }
            plantsIndex.Remove("x");
            Plant.index = plantsIndex;

            //load structures

            temps = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Data\\Structures\\");
            foreach (string i in temps)
            {
                LoadStructure(i);
            }
            structureIndex.Remove("x");
            Structure.index = structureIndex;
        }
        //load animations
        temps = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Data\\Animations\\");
        foreach (string i in temps)
        {
            LoadAnimations(i);
        }
        animationsIndex.Remove("x");
        AnimationsManager.animations = animationsIndex;


    }
    void LoadAnimations(string _path)
    {
        StreamReader reader = File.OpenText(_path);
        string line = "";
        Animation animation = new Animation("x");
        List<Frame> frames = new List<Frame>();
        while ((line = reader.ReadLine()) != null)
        {
            string[] temp = line.Split(':');
            string parameter = temp[0].Trim();
            string argument = temp[1].Trim();
            switch (parameter)
            {
                case "name":
                    {
                        animation.SetFrames(frames.ToArray());
                        frames.Clear();
                        animationsIndex.Add(animation.name, animation);
                        animation = new Animation(argument);
                    }
                    break;
                case "speed":
                    {
                        animation.speed = float.Parse(argument);
                    }
                    break;
                case "parts":
                    {
                        animation.SetObjects(argument.Split(',').Select(x => x.Trim()).ToArray());
                    }
                    break;
                case "frame":
                    {
                        Vector3[] tempPositions, tempRotations;
                        string[] parameters = argument.Split(',');
                        tempPositions = new Vector3[parameters.Length - 1];
                        tempRotations = new Vector3[parameters.Length - 1];
                        for (int i = 1; i < parameters.Length; i++)
                        {
                            tempPositions[i - 1] = ToVector(parameters[i].Split('|')[0]);
                            tempRotations[i - 1] = ToVector(parameters[i].Split('|')[1]);
                        }
                        frames.Add(new Frame(float.Parse(parameters[0]),
                            tempPositions, tempRotations
                            ));
                    }
                    break;
            }

        }
        animation.SetFrames(frames.ToArray());
        animationsIndex.Add(animation.name, animation);

    }
    void LoadStructure(string _path)
    {
        StreamReader reader = File.OpenText(_path);
        string line = "";
        Transform structure = Instantiate(structurePrefab);
        while ((line = reader.ReadLine()) != null)
        {
            string[] temp = line.Split(':');
            string parameter = temp[0].Trim();
            string argument = temp[1].Trim();
            switch (parameter)
            {
                case "name":
                    {
                        if (!structureIndex.ContainsKey(structure.name))
                            structureIndex.Add(structure.name, structure);
                        structure = Instantiate(structurePrefab);
                        structure.name = argument.Split('/')[1].Trim();

                        structure.position = new Vector3(-100, -100, -100);
                        switch (argument.Split('/')[0].Trim())
                        {
                            case "workshop":
                                {
                                    structure.gameObject.AddComponent<WorkShop>();
                                }
                                break;
                        }
                        structure.GetComponent<Structure>().name = argument.Split('/')[1].Trim();
                        structure.GetComponent<Structure>().interaction = structure.Find("Interaction");
                    }
                    break;
                case "needItem":
                    {
                        structure.GetComponent<WorkShop>().neededItem = argument.Trim();
                    }
                    break;
                case "position":
                    {
                        structure.Find("default").localPosition = ToVector(argument);
                    }
                    break;
                case "scale":
                    {
                        structure.Find("default").localScale = ToVector(argument);
                    }
                    break;
                case "interactionPosition":
                    {
                        structure.Find("Interaction").localPosition = ToVector(argument);
                    }
                    break;
                case "interactionRotation":
                    {
                        structure.Find("Interaction").localEulerAngles = ToVector(argument);
                    }
                    break;
                case "animation":
                    {
                        structure.GetComponent<Structure>().useAnimation = argument.Trim();
                    }
                    break;
                case "time":
                    {
                        structure.GetComponent<Structure>().timeAnimation = float.Parse(argument);
                    }
                    break;
                case "grid":
                    {
                        List<Vector3> gridT = new List<Vector3>();
                        string[] l = argument.Trim().Split(' ');
                        for (int i = 0; i < l.Length; i++)
                        {
                            gridT.Add(ToVector(l[i]));
                        }
                        structure.GetComponent<Structure>().grid = gridT.ToArray();
                    }
                    break;
                case "fundation":
                    {
                        List<Vector3> gridT = new List<Vector3>();
                        string[] l = argument.Trim().Split(' ');
                        for (int i = 0; i < l.Length; i++)
                        {
                            gridT.Add(ToVector(l[i]));
                        }
                        structure.GetComponent<Structure>().fundations = gridT.ToArray();
                    }
                    break;
                case "mesh":
                    {
                        structure.Find("default").GetComponent<MeshFilter>().mesh = meshIndex[argument.Trim()];
                        structure.Find("default").GetComponent<MeshCollider>().sharedMesh = meshIndex[argument.Trim()];
                    }
                    break;
                case "items":
                    {
                        structure.GetComponent<WorkShop>().items = argument.Trim().Split(',').Select(x => { return x.Trim(); }).ToArray();
                    }
                    break;
                case "resources":
                    {
                        structure.GetComponent<Structure>().LoadResources(argument.Trim().Split(',').Select(x => { return x.Trim(); }).ToArray());
                    }
                    break;
            }
        }
        if (!structureIndex.ContainsKey(structure.name))
            structureIndex.Add(structure.name, structure);
    }
    void LoadPlant(string _path)
    {
        StreamReader reader = File.OpenText(_path);
        string line = "";
        Plant plant = new Plant("x");
        while ((line = reader.ReadLine()) != null)
        {
            string[] temp = line.Split(':');
            string parameter = temp[0].Trim();
            string argument = temp[1].Trim();
            switch (parameter)
            {
                case "name":
                    {
                        if (!plantsIndex.ContainsKey(plant.name))
                            plantsIndex.Add(plant.name, plant);
                        plant = new Plant(argument.Trim());
                    }
                    break;
                case "growTime":
                    {
                        plant.growTime = float.Parse(argument);
                    }
                    break;
                case "stages":
                    {
                        string[] stages = argument.Split(',');
                        Mesh[] meshes = new Mesh[stages.Length];
                        for (int i = 0; i < stages.Length; i++)
                        {
                            meshes[i] = meshIndex[stages[i].Trim()];
                        }
                        plant.stages = meshes;
                    }
                    break;
            }
        }
        if (!plantsIndex.ContainsKey(plant.name))
            plantsIndex.Add(plant.name, plant);
    }
    void LoadItem(string _path)
    {
        StreamReader reader = File.OpenText(_path);
        string line = "";
        ItemPatern tempPatern = new ItemPatern(containerPrefab, itemPrefab);
        tempPatern.name = "x";

        while ((line = reader.ReadLine()) != null)
        {
            string[] temp = line.Split(':');
            string parameter = temp[0].Trim();
            string argument = temp[1].Trim();
            switch (parameter)
            {

                case "name":
                    {
                        if (!itemsIndex.ContainsKey(tempPatern.name))
                            itemsIndex.Add(tempPatern.name, tempPatern);

                        tempPatern = new ItemPatern(containerPrefab, itemPrefab);
                        tempPatern.name = argument;
                        tempPatern.model = meshIndex["nothing_1"];
                        tempPatern.icon = iconsIndex["nothing"];
                    }
                    break;
                case "size":
                    {
                        tempPatern.size = int.Parse(argument);
                    }
                    break;
                case "tags":
                    {
                        tempPatern.tags = argument.Split(',');
                        for (int w = 0; w < tempPatern.tags.Length; w++)
                        {
                            tempPatern.tags[w] = tempPatern.tags[w].Trim();
                        }
                    }
                    break;
                case "icon":
                    {
                        tempPatern.icon = iconsIndex[argument.Trim()];
                    }
                    break;
                case "model":
                    {
                        tempPatern.model = meshIndex[argument];
                    }
                    break;
                case "resources":
                    {
                        tempPatern.resources = argument.Split(',');
                        for (int w = 0; w < tempPatern.resources.Length; w++)
                        {
                            tempPatern.resources[w] = tempPatern.resources[w].Trim();
                        }
                    }
                    break;
                case "onGround":
                    {
                        string[] positions = argument.Trim().Split(' ');
                        tempPatern.positionOnGround = ToVector(positions[0]);
                        tempPatern.rotationOnGround = ToVector(positions[1]);
                        tempPatern.scaleOnGround = ToVector(positions[2]);
                    }
                    break;
                case "inHand":
                    {
                        string[] positions = argument.Trim().Split(' ');
                        tempPatern.InHand = positions[0];
                        tempPatern.positionInHand = ToVector(positions[1]);
                        tempPatern.rotationInHand = ToVector(positions[2]);
                        tempPatern.scaleInHand = ToVector(positions[3]);
                    }
                    break;
                case "inBackpack":
                    {
                        string[] positions = argument.Trim().Split(' ');
                        tempPatern.InBackpack = positions[0];
                        tempPatern.positionInBackpack = ToVector(positions[1]);
                        tempPatern.rotationInBackpack = ToVector(positions[2]);
                        tempPatern.scaleInBackpack = ToVector(positions[3]);
                    }
                    break;
                case "properties":
                    {
                        Dictionary<string, string> tempDictionary = new Dictionary<string, string>();
                        string[] positions = argument.Trim().Split(',');
                        for (int i = 0; i < positions.Length; i++)
                        {
                            string[] element = positions[i].Split('/');
                            tempDictionary.Add(element[0].Trim(), element[1].Trim());
                        }
                        tempPatern.properties = tempDictionary;
                    }
                    break;
            }
        }
        itemsIndex.Add(tempPatern.name, tempPatern);
    }


    Vector3 ToVector(string _x)
    {
        _x.Trim();
        string[] t = _x.Split('/');
        if (t.Length == 1)
        {
            return new Vector3(float.Parse(t[0]), float.Parse(t[0]), float.Parse(t[0]));
        }
        else
        {
            return new Vector3(float.Parse(t[0]), float.Parse(t[1]), float.Parse(t[2]));
        }

    }
}

