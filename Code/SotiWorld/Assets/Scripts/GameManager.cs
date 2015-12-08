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

public class GameManager : MonoBehaviour
{
    public GameObject character;

	// Use this for initialization
	void Start () {
        BuildMaze();
    }
	
	// Update is called once per frame
	void Update ()
	{
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
                        tileObjects.Add(new IPhoneObject());

                        tileObjects.Add(new TextObject(tile.Text)
                        {
                            Orientation = TextOrientation.East,
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
