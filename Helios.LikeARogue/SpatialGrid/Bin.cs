using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;

namespace Helios.RLToolkit.SpatialGrid
{
    public class Bin
    {
        private List<uint> Entities { get; set; }
        private Rectangle Rect { get; }
        public Bin(int posX, int posY, int cellSize)
        {
            Rect = new Rectangle(posX, posY, cellSize, cellSize);
            Entities = new List<uint>();
        }
    }
}
