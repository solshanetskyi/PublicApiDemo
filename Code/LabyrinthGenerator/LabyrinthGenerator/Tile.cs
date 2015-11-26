namespace LabyrinthGenerator
{
    public class Tile
    {
        public Tile(TileType tileType)
        {
            this.TileType = tileType;
        }

        public TileType TileType { get; set; }
    }
}