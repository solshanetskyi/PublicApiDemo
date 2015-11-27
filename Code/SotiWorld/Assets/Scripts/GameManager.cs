using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Assets.Scripts;
using Integration;

public class GameManager : MonoBehaviour {
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
        PublicApiGateway gateway = new PublicApiGateway();

        var groups = gateway.GetDeviceGroups();

        TextAsset worldMapData = Resources.Load("WorldMap") as TextAsset;
        string[] worldMapLines = worldMapData.text.Split(new[] { "\r\n" }, StringSplitOptions.None);

        int width = worldMapLines.Max(l => l.Length);
        int height = worldMapLines.Length;
        WorldMap worldMap = new WorldMap(width, height);

        for (int y = 0; y < worldMapLines.Length; y++)
        {
            for (int x = 0; x < width && x < worldMapLines[y].Length; x++)
            {
                switch (worldMapLines[y][x])
                {
                    case 'w':
                        worldMap.SetTileObjects(x, y, new WallSection());
                        break;
                    case ' ':
                        worldMap.SetTileObjects(x, y, new FloorSection());
                        break;
                    case 'd':
                        worldMap.SetTileObjects(x, y, new DoorFragment(), new FloorSection());
                        break;
                }
            }
        }

        worldMap.Render();

        character.transform.position = new Vector3(1.5f, 0, 1.5f);
    }
}
