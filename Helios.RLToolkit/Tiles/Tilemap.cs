using Helios.RLToolkit.Gfx;
using SFML.Graphics;
using SFML.System;

namespace Helios.RLToolkit.Tiles
{
    public class Tilemap : Drawable
    {
        private readonly TextureAtlas _atlas;
        private readonly VertexArray _vertexArray;

        private int _width;
        private int _height;
        private readonly float _tileSize;
        private readonly float _cellSize;

        public Tile[,] Tiles { get; }

        public Tilemap(TextureAtlas atlas, int width, int height, float tileTextureDimension, float tileWorldDimension, Tile[,] tiles)
        {
            _atlas = atlas;
            _width = width;
            _height = height;
            _tileSize = tileTextureDimension;
            _cellSize = tileWorldDimension;
            Tiles = tiles;
            _vertexArray = new VertexArray(PrimitiveType.Quads, (uint)(width * height * 4));

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    AddTileVertices(tiles[x, y], new Vector2f(x, y));
                }
            }
        }

       

        public void ClearVA()
        {
            _vertexArray.Clear();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Texture = _atlas.Texture;
            target.Draw(_vertexArray, states);
        }

        public void AddTileVertices(Tile tile, Vector2f position)
        {
            //TODO: store the indices of these 4 verticies on the tile so that they can be
            //accessed and have the color modifired for FOV

            var src = _atlas.GetCoords(tile.Type).Value;

            if (!tile.Cell.IsExplored)
            {
                _vertexArray.Append(new Vertex((new Vector2f(0.0f, 0.0f) + position) * _cellSize,
               Color.Black,
                new Vector2f(_tileSize * src.X, _tileSize * src.Y)));

                _vertexArray.Append(new Vertex((new Vector2f(1.0f, 0.0f) + position) * _cellSize,
                      Color.Black,
                    new Vector2f(_tileSize * src.X + _tileSize, _tileSize * src.Y)));

                _vertexArray.Append(new Vertex((new Vector2f(1.0f, 1.0f) + position) * _cellSize,
                      Color.Black,
                    new Vector2f(_tileSize * src.X + _tileSize, _tileSize * src.Y + _tileSize)));

                _vertexArray.Append(new Vertex((new Vector2f(0.0f, 1.0f) + position) * _cellSize,
                    Color.Black,
                    new Vector2f(_tileSize * src.X, _tileSize * src.Y + _tileSize)));
            }
            else
            {
                if (!tile.Cell.IsInFov)
                {
                    var colorOffset = new Color(133, 133, 133);
                    _vertexArray.Append(new Vertex((new Vector2f(0.0f, 0.0f) + position) * _cellSize,
                        tile.Tint * colorOffset,
                        new Vector2f(_tileSize * src.X, _tileSize * src.Y)));

                    _vertexArray.Append(new Vertex((new Vector2f(1.0f, 0.0f) + position) * _cellSize,
                        tile.Tint * colorOffset,
                        new Vector2f(_tileSize * src.X + _tileSize, _tileSize * src.Y)));

                    _vertexArray.Append(new Vertex((new Vector2f(1.0f, 1.0f) + position) * _cellSize,
                        tile.Tint * colorOffset,
                        new Vector2f(_tileSize * src.X + _tileSize, _tileSize * src.Y + _tileSize)));

                    _vertexArray.Append(new Vertex((new Vector2f(0.0f, 1.0f) + position) * _cellSize,
                        tile.Tint * colorOffset,
                        new Vector2f(_tileSize * src.X, _tileSize * src.Y + _tileSize)));
                }
                else
                {
                    _vertexArray.Append(new Vertex((new Vector2f(0.0f, 0.0f) + position) * _cellSize,
                        tile.Tint,
                        new Vector2f(_tileSize * src.X, _tileSize * src.Y)));

                    _vertexArray.Append(new Vertex((new Vector2f(1.0f, 0.0f) + position) * _cellSize,
                        tile.Tint,
                        new Vector2f(_tileSize * src.X + _tileSize, _tileSize * src.Y)));

                    _vertexArray.Append(new Vertex((new Vector2f(1.0f, 1.0f) + position) * _cellSize,
                        tile.Tint,
                        new Vector2f(_tileSize * src.X + _tileSize, _tileSize * src.Y + _tileSize)));

                    _vertexArray.Append(new Vertex((new Vector2f(0.0f, 1.0f) + position) * _cellSize,
                        tile.Tint,
                        new Vector2f(_tileSize * src.X, _tileSize * src.Y + _tileSize)));
                }
            }
        }
    }
}
