using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.LabyrinthGeneration
{
    public class Room
    {
        private Matrix _matrix;
        private string _name;
        private string _color;

        public Room(int width, string name, string color, List<DeviceInfo> deviceInfos)
        {
            _name = name;
            _color = color;

            this._matrix = new Matrix(width, Settings.DefaultHeight + Settings.RoomCoridorLenght);

            int firstEntranceWall = this._matrix.Tiles.Count / 2 - 1* Settings.Scale;
            int secondEntranceWall = this._matrix.Tiles.Count / 2 + 1 * Settings.Scale;

            int doorCenter = firstEntranceWall + (secondEntranceWall - firstEntranceWall)/2;

            for (int i = 0; i < this._matrix.Tiles.Count; i++)
            {
                for (int j = 0; j < this._matrix.Tiles[i].Count; j++)
                {
                    //Drawing a corridor in front of the door entrance
                    if ((i == 0 && j > Settings.RoomCoridorLenght) || j == Settings.RoomCoridorLenght
                        || (i == _matrix.Tiles.Count - 1 && j > Settings.RoomCoridorLenght)
                        || j == _matrix.Tiles[i].Count - 1)
                    {
                        _matrix.Tiles[i][j].Add(new Tile(TileType.Wall));
                    }

                    //Drawing door at the start and end of the corridor
                    if (j >= 0 && j <= Settings.RoomCoridorLenght && i > firstEntranceWall && i < secondEntranceWall)
                    {
                        _matrix.Tiles[i][j].Add(new Tile(TileType.Door));
                    }

                    if ((i == firstEntranceWall && j < Settings.RoomCoridorLenght) || (i == secondEntranceWall && j < Settings.RoomCoridorLenght))
                    {
                        _matrix.Tiles[i][j].Add(new Tile(TileType.Wall));
                    }

                    //Room name
                    if (i == doorCenter && j == 0)
                    {
                        var tile = new Tile(TileType.Text);
                        tile.Text = _name;
                        tile.Color = _color;

                        _matrix.Tiles[i][j].Add(tile);
                    }

                    bool hasWalls = _matrix.Tiles[i][j].Any(t => t.TileType == TileType.Wall);

                    if (!hasWalls)
                    {
                        _matrix.Tiles[i][j].Add(new Tile(TileType.Floor));
                    }
                }
            }

            InstallDevicesOnWall(deviceInfos.Take(deviceInfos.Count /2).ToList(), TextOrientation.East);
            InstallDevicesOnWall(deviceInfos.Skip(deviceInfos.Count/2).ToList(), TextOrientation.West);
        }

        private void InstallDevicesOnWall(List<DeviceInfo> deviceInfos, TextOrientation orientation)
        {
            if (!deviceInfos.Any())
                return;

            int xPosition;

            switch (orientation)
            {
                case TextOrientation.East:
                    xPosition = 0;
                    break;
                case TextOrientation.West:
                    xPosition = _matrix.Tiles.Count - 1;
                    break;
                default:
                    throw new NotImplementedException();
            }

            int phoneIndex = 0;
            int phonePosition = 0;

            while (phoneIndex != deviceInfos.Count)
            {
                phonePosition = (phoneIndex + 1) * (_matrix.Tiles[xPosition].Count - Settings.RoomCoridorLenght) / (deviceInfos.Count + 1) + Settings.RoomCoridorLenght;

                AddDeviceToWall(xPosition, phonePosition, deviceInfos[phoneIndex].DeviceId, orientation);
                phoneIndex++;
            }}

        public void AddDeviceToWall(int x, int y, string name, TextOrientation orientation)
        {
            _matrix.Tiles[x][y].Add(new Tile(TileType.Iphone)
            {
                Text = name,
                Orientation = orientation
            });
        }

        public Matrix AsMatrix()
        {
            return this._matrix;
        }
    }
}