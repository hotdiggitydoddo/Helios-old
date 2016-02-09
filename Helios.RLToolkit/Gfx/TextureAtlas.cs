using System.Collections.Generic;
using Helios.RLToolkit.Generators;
using SFML.Graphics;
using SFML.System;

namespace Helios.RLToolkit.Gfx
{
    public class TextureAtlas
    {
        public Dictionary<TileType, Vector2f> Items { get; }
        public Texture Texture { get; }
        public int SpriteSize { get; }
        public TextureAtlas(Texture tex, int spriteSize)
        {
            Items = new Dictionary<TileType, Vector2f>();
            Texture = tex;
            SpriteSize = spriteSize;
        }

        public Vector2f? GetCoords(TileType type)
        {
            return Items.ContainsKey(type) ? Items[type] : (Vector2f?) null;
        }
    }
}
