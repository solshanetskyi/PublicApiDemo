using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Integration;
using Assets.Scripts.LabyrinthGeneration;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour
{
    public GameObject character;
    public GameObject mainMenu;
    public GameObject deviceManagementMenu;
    public GameObject deviceManagementPrompt;

    // Use this for initialization
    void Start () {
        BuildMaze();
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (deviceManagementMenu.activeSelf)
            {
                deviceManagementMenu.SetActive(false);
                character.GetComponent<FirstPersonController>().enabled = true;
            }
            else
            {
                mainMenu.SetActive(!mainMenu.activeSelf);
                Time.timeScale = mainMenu.activeSelf ? 0 : 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && Game.ActiveDevice != null)
        {
            character.GetComponent<FirstPersonController>().enabled = false;
            deviceManagementPrompt.SetActive(false);
            deviceManagementMenu.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        Game.Refresh();

        Time.timeScale = 1;
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void NewGame()
    {
        Time.timeScale = 1;
        Application.LoadLevel("Castle");
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        Application.Quit();
    }

    private void BuildMaze()
    {
        Matrix labyrinthMatrix = Game.LabyrinthMatrix;

        int width = labyrinthMatrix.Tiles.Max(l => l.Count);
        int height = labyrinthMatrix.Tiles.Count;

        WorldMap worldMap = new WorldMap(width, height);

        for (int y = 0; y < labyrinthMatrix.Tiles.Count; y++)
        {
            for (int x = 0; x < width && x < labyrinthMatrix.Tiles[y].Count; x++)
            {
                var tiles = labyrinthMatrix.Tiles[y][x];

                List<ITileObject> tileObjects = new List<ITileObject>();

                foreach (Tile tile in tiles)
                {
                    if (tile.TileType == TileType.Wall)
                    {
                        tileObjects.Add(new WallSection());
                    }
                    else if (tile.TileType == TileType.Floor)
                    {
                        tileObjects.Add(new FloorSection());
                    }
                    else if (tile.TileType == TileType.Door)
                    {
                        tileObjects.Add(new DoorFragment());
                    }
                    else if (tile.TileType == TileType.Text)
                    {
                        tileObjects.Add(new TextObject(tile.Text)
                        {
                            Orientation = TextOrientation.South,
                            Altitude = 5,
                            TextColor = GetTextColorFromString(tile.Color)
                        });
                    }
                    else if (tile.TileType == TileType.Iphone)
                    {
                        string deviceName = Game.GetDeviceById(tile.Text).Name;

                        tileObjects.Add(new IPhoneObject() { Orientation = tile.Orientation, DeviceId = tile.Text});

                        tileObjects.Add(new TextObject(deviceName)
                        {
                            Orientation = tile.Orientation,
                            Altitude = 5.5f,
                            TextColor = GetTextColorFromString("Yellow")
                        });
                    }
                    else if (tile.TileType == TileType.Player)
                    {
                        character.transform.position = new Vector3(y, -5, x);
                    }

                    worldMap.SetTileObjects(x, y, tileObjects.ToArray());
                }
            }
        }

        UpdateStatistics();
        worldMap.Render();
    }

    private void UpdateStatistics()
    {
        var totalGroupsLabel = GameObject.Find("TotalGroups").GetComponent<Text>();
        var totalDevicesLabel = GameObject.Find("TotalDevices").GetComponent<Text>();

        totalGroupsLabel.text = String.Format("Total Groups: {0}", Game.TotalGroups);
        totalDevicesLabel.text = String.Format("Total Devices: {0}", Game.TotalDevices);
    }

    private TextColor GetTextColorFromString(string textColorString)
    {
        if (!Enum.IsDefined(typeof(TextColor), textColorString))
            return TextColor.Cyan;

        return (TextColor)Enum.Parse(typeof (TextColor), textColorString);
    }
}
