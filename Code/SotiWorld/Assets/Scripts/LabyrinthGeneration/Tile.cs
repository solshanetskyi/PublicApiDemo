namespace Assets.Scripts.LabyrinthGeneration
{
    public class Tile
    {
        public Tile(TileType tileType)
        {
            this.TileType = tileType;
        }

        public string Text { get; set; }

        public TileType TileType { get; set; }
    }
}