namespace LabyrinthGenerator
{
    public class Room
    {
        private Matrix _matrix;

        public Room(int width)
        {
            this._matrix = new Matrix(width, Settings.DefaultHeight + Settings.RoomCoridorLenght);

            int firstEntranceWall = this._matrix.Tiles.Count / 2 - 1* Settings.Scale;
            int secondEntranceWall = this._matrix.Tiles.Count / 2 + 1 * Settings.Scale;

            for (int i = 0; i < this._matrix.Tiles.Count; i++)
            {
                for (int j = 0; j < this._matrix.Tiles[i].Count; j++)
                {
                    if ((i == 0 && j > Settings.RoomCoridorLenght) || j == Settings.RoomCoridorLenght
                        || (i == _matrix.Tiles.Count - 1 && j > Settings.RoomCoridorLenght)
                        || j == _matrix.Tiles[i].Count - 1)
                    {
                        _matrix.Tiles[i][j].TileType = TileType.Wall;
                    }

//                    if (i == 0 || j == Settings.RoomCoridorLenght || i == this._matrix.Tiles.Count - 1 || j == this._matrix.Tiles[i].Count - 1)
//                    {
//                        this._matrix.Tiles[i][j].TileType = TileType.Wall;
//                    }

                    if ((j == 0 || j == Settings.RoomCoridorLenght) && i > firstEntranceWall && i < secondEntranceWall)
                    {
                        _matrix.Tiles[i][j].TileType = TileType.Door;
                    }

                    if ((i == firstEntranceWall && j < Settings.RoomCoridorLenght) || (i == secondEntranceWall && j < Settings.RoomCoridorLenght))
                    {
                        _matrix.Tiles[i][j].TileType = TileType.Wall;
                    }
                }
            }
        }

        public Matrix AsMatrix()
        {
            return this._matrix;
        }
    }
}

//namespace LabyrinthGenerator
//{
//    public class Room
//    {
//        private Matrix _matrix;
//
//        public Room(int width)
//        {
//            this._matrix = new Matrix(width, Settings.DefaultHeight);
//
//            int firstEntranceWall = this._matrix.Tiles.Count / 2 - 1 * Settings.Scale;
//            int secondEntranceWall = this._matrix.Tiles.Count / 2 + 1 * Settings.Scale;
//
//
//            for (int i = 0; i < this._matrix.Tiles.Count; i++)
//            {
//                for (int j = 0; j < this._matrix.Tiles[i].Count; j++)
//                {
//                    //                        if ((i == 0 && j > Settings.RoomCoridorLenght) || j == Settings.RoomCoridorLenght || (i == _matrix.Tiles.Count - 1 && j > Settings.RoomCoridorLenght) || j == _matrix.Tiles[i].Count - 1)
//                    //                        {
//                    //                            _matrix.Tiles[i][j].TileType = TileType.Wall;
//                    //                        }
//
//                    if (i == 0 || j == 0 || i == this._matrix.Tiles.Count - 1 || j == this._matrix.Tiles[i].Count - 1)
//                    {
//                        this._matrix.Tiles[i][j].TileType = TileType.Wall;
//                    }
//
//                    //                        if (j == Settings.RoomCoridorLenght && i > firstEntranceWall && i < secondEntranceWall)
//                    //                        {
//                    //                            _matrix.Tiles[i][j].TileType = TileType.Door;
//                    //                        }
//                    //
//                    //                        if ((i == firstEntranceWall && j < Settings.RoomCoridorLenght) || (i == secondEntranceWall && j < Settings.RoomCoridorLenght))
//                    //                        {
//                    //                            _matrix.Tiles[i][j].TileType = TileType.Wall;
//                    //                        }
//                }
//            }
//        }
//
//        public Matrix AsMatrix()
//        {
//            return this._matrix;
//        }
//    }
//}