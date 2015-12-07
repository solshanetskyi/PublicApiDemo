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

public class GameManager : MonoBehaviour
{
    private const string ClientSecret = "ClientSecret";

    private const string OwlUrl = "https://owl.corp.soti.net/mobicontrol/api";
    private const string OwlClientId = "48c28dd54368439886ab7663389d087c";

    private const string CaimanUrl = "https://caiman.corp.soti.net/mobicontrol/api";
    private const string CaimanClientId = "48c28dd54368439886ab7663389d087c";

    private const string TestEnvironmentUrl = "https://qa-ms7e64-nav.sotiqa.com/MobiControl/api";
    private const string TestEnvironmentClientId = "910f8093e02f44958f47b984ca1e6a5e";

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
        //PublicApiGateway gateway = new PublicApiGateway(OwlUrl, OwlClientId, ClientSecret);

        PublicApiGatewayMock gateway = new PublicApiGatewayMock();
        gateway.Login("Administrator", "1");

        var deviceGroups = gateway.GetDeviceGroups();

        Matrix labyrinthMatrix = MapGenerator.GenerateMap(deviceGroups);

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

                        tileObjects.Add(new TextObject("Ipad's Sergii")
                        {
                            Orientation = TextOrientation.East,
                            Altitude = 5.5f,
                            TextColor = GetTextColorFromString("Cyan")
                        });
                    }

                    worldMap.SetTileObjects(x, y, tileObjects.ToArray());
                }
            }
        }

        worldMap.Render();

        character.transform.position = new Vector3(width/2 + 6, -5, 0);
    }

    private TextColor GetTextColorFromString(string textColorString)
    {
        if (!Enum.IsDefined(typeof(TextColor), textColorString))
            return TextColor.Cyan;

        return (TextColor)Enum.Parse(typeof (TextColor), textColorString);
    }
}
