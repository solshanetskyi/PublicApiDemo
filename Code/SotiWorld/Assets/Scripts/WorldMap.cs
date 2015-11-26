using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class WorldMap
    {
        private readonly List<ITileObject>[,] _tileObjects;

        public int Width { get; private set; }

        public int Height { get; private set; }

        public WorldMap(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new InvalidOperationException("A level must have positive dimensions");

            Width = width;
            Height = height;
            _tileObjects = new List<ITileObject>[Width, Height];
        }

        public ITileObject[] GetTileObjects(int x, int y)
        {
            if (x < 0 || x > Width - 1)
                throw new ArgumentOutOfRangeException("x");
            if (y < 0 || y > Height - 1)
                throw new ArgumentOutOfRangeException("y");

            if (_tileObjects[x, y] == null)
                return new ITileObject[0];

            return _tileObjects[x, y].ToArray();
        }

        public void SetTileObjects(int x, int y, params ITileObject[] tileObjectList)
        {
            if (x < 0 || x > Width - 1)
                throw new ArgumentOutOfRangeException("x");
            if (y < 0 || y > Height - 1)
                throw new ArgumentOutOfRangeException("y");

            _tileObjects[x, y] = tileObjectList.ToList();
        }

        public void AddTileObjects(int x, int y, params ITileObject[] tileObjectList)
        {
            if (x < 0 || x > Width - 1)
                throw new ArgumentOutOfRangeException("x");
            if (y < 0 || y > Height - 1)
                throw new ArgumentOutOfRangeException("y");

            if (_tileObjects[x, y] == null)
                _tileObjects[x, y] = tileObjectList.ToList();
            else
                _tileObjects[x, y].AddRange(tileObjectList);
        }

        public void Render()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var tileObjects = _tileObjects[i, j];

                    if (tileObjects == null)
                        continue;

                    foreach (var tileObject in tileObjects)
                        tileObject.Render(j, 0, i);
                }
            }
        }
    }
}