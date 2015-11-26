namespace LabyrinthGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class Matrix
    {
        public Matrix()
        {
            this._tiles = new List<List<Tile>>();
        }

        public Matrix(int width, int height)
        {
            this._tiles = new List<List<Tile>>();

            for (int i = 0; i < width; i++)
            {
                var list = new List<Tile>();
                this._tiles.Add(list);

                for (int j = 0; j < height; j++)
                {
                    this._tiles[i].Add(new Tile(TileType.None));
                }
            }
        }

        private List<List<Tile>> _tiles;

        public List<List<Tile>> Tiles
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
//                Console.Write(i + " ");

                for (int j = 0; j < this._tiles[i].Count; j++)
                {
                    switch (this._tiles[i][j].TileType)
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

        public void Merge(Matrix matrix, int x, int y)
        {
            for (int i = 0; i < matrix.Tiles.Count; i++)
            {
                for (int j = 0; j < matrix.Tiles[i].Count; j++)
                {
                    if (matrix.Tiles[i][j].TileType != TileType.None)
                        this.Tiles[i + x][j + y] = matrix.Tiles[i][j];
                }
            }
            
        }
    }
}