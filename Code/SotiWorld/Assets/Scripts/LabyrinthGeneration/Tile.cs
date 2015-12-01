namespace Assets.Scripts.LabyrinthGeneration
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