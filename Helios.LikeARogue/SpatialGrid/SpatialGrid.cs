using System.Collections.Generic;
using System.Linq;
using Helios.LikeARogue.Generators;
using Helios.RLToolkit.SpatialGrid;

namespace Helios.LikeARogue.SpatialGrid
{
    public class SpatialGrid
    {
        //private int _columns;
        //private int _rows;
        //private int _cellSize;
        //private GameWorld _world;

        //public List<Bin> Grid { get; }

        //void Initialize(GameWorld world)
        //{
        //    _world = world;
        //    _cellSize = world.CurrentGameLevel.CellSize;
        //    _columns = world.CurrentGameLevel.WidthInPixels/_cellSize;
        //    _rows = world.CurrentGameLevel.HeightInPixels/_cellSize;

        //    CreateBins();
        //}

        //void CreateBins()
        //{
        //    for (var y = 0; y < _rows; y++)
        //    {
        //        for (var x = 0; x < _columns; x++)
        //        {
        //            Bin bin = new Bin(x * _cellSize, y * _cellSize, _cellSize);
        //            Grid.Add(bin);
        //        }
        //    }
        //}

        //public void RegisterEntities()
        //{
        //    foreach (var entity in _world.CurrentGameLevel.GetAllEntities())
        //    {
        //        RegisterEntity(entity);
        //    }
        //}

        //private void RegisterEntity(uint entity)
        //{
        //    var spatial = _world.SpatialComponents.SingleOrDefault(x => x.Owner == entity);
        //    if (spatial == null) return;

        //    var collision = _world.CollisionComponents.SingleOrDefault(x => x.Owner == entity);
        //    if (collision == null) return;

        //    var tile = _world.CurrentGameLevel.GetTile(spatial.Position);
        //   // var 

        //}
    }
}
