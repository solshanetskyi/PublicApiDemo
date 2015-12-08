namespace Assets.Scripts.LabyrinthGeneration
{
    public class Tile
    {
        public Tile(TileType tileType)
        {
            this.TileType = tileType;
        }

        public string Text { get; set; }

        public string Color { get; set; }

        public TileType TileType { get; set; }

        public TextOrientation Orientation { get; set; }
    }
}