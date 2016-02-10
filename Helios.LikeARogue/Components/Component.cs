using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helios.LikeARogue.Components
{
    public abstract class Component
    {
        public uint Owner { get; set; }
    }
}
