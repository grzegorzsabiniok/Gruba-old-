using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public float menuSpeed;
    public Toggle fullscreen;
    public Dropdown resolution;
    public RectTransform main, options;
    public int selectedMenu = 0;
    public RectTransform[] panels;
    public Vector3[] positions;

    int width, height;
    public void Exit()
    {
        Application.Quit();
    }
    public void NewGame()
    {
        PlayerPrefs.SetInt("mapSeed", (int)Time.time);
        SceneManager.LoadScene("main");
    }
    public void ChangeMenu(int nr)
    {
        selectedMenu = nr;
    }
    public void SaveOptions()
    {
        string[] temp = resolution.captionText.text.Split('X');
        Screen.SetResolution(int.Parse(temp[0]), int.Parse(temp[1]), fullscreen.isOn);
        PlayerPrefs.SetString("resolution", resolution.captionText.text);
        PlayerPrefs.SetInt("fullscreen", fullscreen.isOn ? 1 : 0);
    }

    public void ChangeResolution(int _width, int _height)
    {
        width = _width;
        height = _height;
    }
    void Update()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            if (i == selectedMenu)
            {
                panels[i].anchoredPosition = Vector3.MoveTowards(panels[i].anchoredPosition, Vector3.zero, menuSpeed * 1000 * Time.deltaTime);
            }
            else
            {
                panels[i].anchoredPosition = Vector3.MoveTowards(panels[i].anchoredPosition, new Vector3(positions[i].x * Screen.width * 2, positions[i].y * Screen.height * 2, 0), menuSpeed * 1000 * Time.deltaTime);
            }
        }
    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("fullscreen"))
        {
            PlayerPrefs.SetInt("fullscreen", 1);
        }
        if (!PlayerPrefs.HasKey("resolution"))
        {
            PlayerPrefs.SetString("resolution", Screen.width + " X " + Screen.height);
        }
        fullscreen.isOn = PlayerPrefs.GetInt("fullscreen") == 1;
        resolution.captionText.text = PlayerPrefs.GetString("resolution");
        string[] temp = resolution.captionText.text.Split('X');
        Screen.SetResolution(int.Parse(temp[0]), int.Parse(temp[1]), fullscreen.isOn);
    }

}
