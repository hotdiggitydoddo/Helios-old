using Helios.RLToolkit.Generators;
using RogueSharp;
using SFML.Graphics;

namespace Helios.RLToolkit.Tiles
{
    public class Tile 
    {
        private uint? _entity;
        public TileType Type { get; set; }
        public Color Tint { get; set; }
        public uint? Entity { get { return _entity; } set { _entity = value; } }
        public bool IsEmpty => Type != TileType.Floor && _entity == null;
        public Cell Cell { get; set; }
        public Tile() { }

        public Sprite Sprite;

        public Tile(TileType type, Color tint = default(Color), uint? entity = null)
        {
            Type = type;
            Tint = tint;
            _entity = entity;
        }
    }
}
