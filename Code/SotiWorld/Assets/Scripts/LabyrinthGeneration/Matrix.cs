using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.LabyrinthGeneration
{
    public class Matrix
    {
        public Matrix()
        {
            this._tiles = new List<List<List<Tile>>>();
        }

        public Matrix(int width, int height)
        {
            this._tiles = new List<List<List<Tile>>>();

            for (int i = 0; i < width; i++)
            {
                var doubleList = new List<List<Tile>>();
                _tiles.Add(doubleList);

                for (int j = 0; j < height; j++)
                {
                    doubleList.Add(new List<Tile>() {new Tile(TileType.Floor)});
                }
            }
        }

        private List<List<List<Tile>>> _tiles;

        public List<List<List<Tile>>> Tiles
        {
            get
            {
                return this._tiles;
            }
        }

        public void Print()
        {
            for (int i = 0; i < this._tiles.Count; i++)
            {
                for (int j = 0; j < this._tiles[i].Count; j++)
                {
                    switch (this._tiles[i][j][0].TileType)
                    {
                        case TileType.None:
                            Console.Write(" ");
                            break;
                        case TileType.Wall:
                            Console.Write("w");
                            break;
                        case TileType.Door:
                            Console.Write("d");
                            break;
                        default:
                            throw new AccessViolationException();
                    }
                }

                Console.WriteLine();
            }
        }

        public string AsString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < this._tiles.Count; i++)
            {
                for (int j = 0; j < this._tiles[i].Count; j++)
                {
                    switch (this._tiles[i][j][0].TileType)
                    {
                        case TileType.None:
                            stringBuilder.Append(" ");
                            break;
                        case TileType.Wall:
                            stringBuilder.Append("w");
                            break;
                        case TileType.Door:
                            stringBuilder.Append("d");
                            break;
                        default:
                            throw new AccessViolationException();
                    }
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        public void Merge(Matrix matrix, int x, int y)
        {
            for (int i = 0; i < matrix.Tiles.Count; i++)
            {
                for (int j = 0; j < matrix.Tiles[i].Count; j++)
                {
                    if (matrix.Tiles[i][j].Count > 0)
                    {
                        Tiles[i + x][j + y].AddRange(matrix.Tiles[i][j]);

                        bool hasDoors = Tiles[i + x][j + y].Any(t => t.TileType == TileType.Door);

                        if (hasDoors)
                        {
                            Tiles[i + x][j + y].RemoveAll(t => t.TileType == TileType.Wall);
                        }
                    }
                }
            }
        }
    }
}