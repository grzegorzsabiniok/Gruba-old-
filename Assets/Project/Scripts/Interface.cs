using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour {
    public Transform topRenderUi, menu, structure;
    public Slider slider;
    public Text speedText;
    public Material normal, wrong, good;
    public static Interface main;
    public LayerMask Layer;
    public Transform shortcut;
    public string plant = "";
    public bool DevMode = true;
    int unitCount = 0;
    int fps;
    void Start () {
        main = this;
        topRenderUi.GetComponent<Text>().text = Main.main.topRender.ToString();
        
        slider.maxValue = Main.main.chunkSize.y*Main.main.mapSize.y-1;
        slider.value = Main.main.topRender;
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Menu()
    {
        Application.LoadLevel(0);
    }
    int actualSpeedLevel = 2;
    float[] speedLevels = new float[]
    {
        0,0.5f,1,2,5,10
    };
    public void GameSpeed(int _mod)
    {
        int temp = actualSpeedLevel + _mod;
        if (temp < 0) temp = 0;
        if (temp > speedLevels.Length - 1) temp = speedLevels.Length - 1;
        actualSpeedLevel = temp;
        Main.main.SetSpeed(speedLevels[temp]);
        speedText.text = "X "+Main.main.gameSpeed.ToString();
    }
    public void Build(string target)
    {
        structure = Instantiate(Structure.index[target]);
        structure.position = Vector3.zero;
    }
    public void Build3(Transform _structure)
    {
        if (_structure.GetComponent<Unit>())
        {
            if (unitCount < 100)
            {
                Random.seed = (int)Time.time;
                int temp = Random.Range(0, Main.main.namesList.Count - 1);
                string temp2 = Main.main.namesList[temp];
                //Main.main.namesList.RemoveAt(temp);
                _structure.GetComponent<Unit>().firstName = temp2.Split(' ')[0];
                _structure.GetComponent<Unit>().lastName = temp2.Split(' ')[1];
                unitCount++;
                structure = Instantiate(_structure);
                structure.position = Vector3.zero;
            }
        }
        else
        {
            structure = Instantiate(_structure);
            structure.position = Vector3.zero;
        }
    }
    
    void OnGUI()
    {
        if(DevMode)
        GUI.Label(new Rect(0, 0, 50, 20), fps.ToString());
    }
    int savedTime = 0;
    Field savedField;
    void Update()
    {
        if(savedTime != Mathf.Floor(Time.time)){
            savedTime = (int)Mathf.Floor(Time.time);
            fps = (int)(1 / Time.smoothDeltaTime);
        }
        
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Build("carpenter");
        }
        if (plant != "")
        {

                RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                if (hit.transform.tag == "field")
                {
                    if (savedField != null)
                        savedField.Deselect();
                    savedField = hit.transform.parent.GetComponent<Field>();
                    savedField.Select();
                    if (Input.GetMouseButtonDown(0))
                    {
                        hit.transform.parent.GetComponent<Field>().ChangePlant(plant);
                        plant = "";
                    }
                }
                else
                {
                    if (savedField != null)
                        savedField.Deselect();
                }
            }
            else
            {
                if (savedField != null)
                    savedField.Deselect();
            }
            if (Input.GetMouseButtonDown(1))
            {
                plant = "";
                if (savedField != null)
                    savedField.Deselect();
            }
        }
    
        if (structure != null)
        {
            if (structure.GetComponent<Unit>())
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray.origin, ray.direction, out hit))
                {
                    if (hit.transform.tag == "terrain")
                    {
                        structure.position = Main.Normalize(hit.point - (hit.point - ray.origin) / 1000);

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (Go.CanGo(structure.position))
                            {
                                structure.GetComponent<Unit>().Born();
                                structure.GetComponent<Unit>().ready = true;
                                structure = null;
                            }
                        }
                    }
                    }
                }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray.origin, ray.direction, out hit))
                {
                    
                    if (Input.GetKeyDown("r"))
                    {
                        structure.GetComponent<Structure>().Rotate(1);
                    }


                    if (hit.transform.tag == "terrain")
                    {
                        structure.position = Main.Normalize(hit.point - (hit.point - ray.origin) / 1000);
                    }
                    if (structure.GetComponent<Structure>().CanPlace())
                    {
                        structure.Find("default").GetComponent<MeshRenderer>().material = good;
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (structure.Find("default").GetComponent<MeshCollider>())
                                structure.Find("default").GetComponent<MeshCollider>().enabled = true;
                            structure.Find("default").GetComponent<MeshRenderer>().material = normal;
                            structure.GetComponent<Structure>().GenerateColiders();
                            structure.GetComponent<Structure>().PlacePlan();
                            structure = null;
                        }
                    }
                    else
                    {
                        structure.Find("default").GetComponent<MeshRenderer>().material = wrong;
                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        Destroy(structure.gameObject);
                        structure = null;
                    }
                }
            }
        }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menu.gameObject.active = !menu.gameObject.active;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Main.main.topRender++;
                Chunk[] temp = Main.main.GetAllChunks();
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i].sliced = true;
                }
                topRenderUi.GetComponent<Text>().text = Main.main.topRender.ToString();
            }
        if (Input.GetKeyDown(KeyCode.Q))
        {
                Main.main.topRender--;
                Chunk[] temp = Main.main.GetAllChunks();
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i].sliced = true;
                }
                topRenderUi.GetComponent<Text>().text = Main.main.topRender.ToString();
            }
        
    }
    public void TerrainRender()
    {
        Main.main.topRender = (int)slider.value;
        Chunk[] temp = Main.main.GetAllChunks();
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].sliced = true;
        }
        topRenderUi.GetComponent<Text>().text = Main.main.topRender.ToString();
    }
    public void Stockpile()
    {

    }
    
}
