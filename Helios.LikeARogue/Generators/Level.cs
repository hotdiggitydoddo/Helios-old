using System;
using System.Collections.Generic;
using System.Linq;
using Helios.RLToolkit.Gfx;
using Helios.RLToolkit.Tiles;
using RogueSharp;
using SFML.Graphics;
using SFML.System;
using Helios.RLToolkit.Generators;

namespace Helios.LikeARogue.Generators
{
    public class Level : Drawable
    {
        public uint Player { get; set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public PathFinder PathFinder { get; protected set; }

        //public static readonly Camera Camera = new Camera();

        protected IMap _map;
        protected Tilemap Tilemap;
        protected List<uint> _gameObjects;
        protected GameWorld _world;

        private Level(GameWorld world, IMap map, Tilemap tilemap)
        {
            _world = world;
            _map = map;
            Tilemap = tilemap;
            _gameObjects = new List<uint>();
            Width = map.Width;
            Height = map.Height;
            PathFinder = new PathFinder(map);
        }

        public static Level Generate(GameWorld world, int width, int height, TextureAtlas atlas, int cellSize)
        {
            DungeonGenerator.Instance.GenerateHauberkDungeon(width, height, 195000, 5, 5, 50, false, true);

            var tileData = DungeonGenerator._dungeon;
            var map = new Map(width, height);
            var tiles = new Tile[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    Color color = Color.White;

                    switch (tileData[x, y])
                    {
                        case TileType.Floor:
                            map.SetCellProperties(x, y, true, true);
                            color = new Color(194, 194, 194);
                            break;
                        case TileType.RoomFloor:
                            map.SetCellProperties(x, y, true, true);
                            color = new Color(35, 255, 255);
                            break;
                        case TileType.Wall:
                            map.SetCellProperties(x, y, false, false);
                            color = new Color(163, 163, 163);
                            break;
                        case TileType.Door:
                            map.SetCellProperties(x, y, false, true);
                            color = new Color(132, 81, 0);
                            break;
                    }

                    tiles[x, y] = new Tile(tileData[x, y], color);
                    tiles[x, y].Cell = map.GetCell(x, y);
                }
            }

            var tilemap = new Tilemap(atlas, width, height, atlas.SpriteSize, cellSize, tiles);

            return new Level(world, map, tilemap);
        }

        public Tile GetTile(Vector2f position)
        {
            var tile = Tilemap.Tiles[(int)position.X, (int)position.Y];
            tile.Cell = _map.GetCell((int)position.X, (int)position.Y);
            return tile;
        }

        public Tile GetTile(int x, int y)
        {
            var tile = Tilemap.Tiles[x, y];
            tile.Cell = _map.GetCell(x, y);
            return tile;
        }

        public Tile SetTileProperties(Tile tile, bool isWalkable, bool isTransparent, bool isExplored)
        {
            _map.SetCellProperties(tile.Cell.X, tile.Cell.Y, isTransparent, isWalkable, isExplored);
            return GetTile(tile.Cell.X, tile.Cell.Y);
        }

        public void UpdatePlayerFov(Vector2f position)
        {
            _map.ComputeFov((int)position.X, (int)position.Y, 30, true);
            Tilemap.ClearVA();

            foreach (var cell in _map.GetAllCells())
            {
                if (_map.IsInFov(cell.X, cell.Y))
                {
                   _map.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
                Tilemap.AddTileVertices(GetTile(cell.X, cell.Y), new Vector2f(cell.X, cell.Y));
            }
        }

        public Tile[] GetSurroundingTiles(Vector2f position, int distance = 1)
        {
            var cells = _map.GetBorderCellsInArea((int) position.X, (int) position.Y, distance).ToArray();
            var tiles = new Tile[cells.Length];
            for (var i = 0; i < cells.Length; i++)
                tiles[i] = GetTile(cells[i].X, cells[i].Y);
            return tiles;
        }

        public Tile GetRandomEmptyTile()
        {
            var random = new Random();

            while (true)
            {
                int x = random.Next(_map.Width);
                int y = random.Next(_map.Height);

                var tile = GetTile(x, y);
                if (tile.Cell.IsWalkable)
                    return tile;
            }
        }

        public Tile[,] GetAllTiles()
        {
            return Tilemap.Tiles;
        }

        public bool IsInFov(Vector2f position)
        {
            return _map.IsInFov((int)position.X, (int)position.Y);
        }

      

        public void Draw(RenderTarget target, RenderStates states)
        {
            Tilemap.Draw(target, states);
        }
    }
}
