using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using Key = SFML.Window.Keyboard.Key;

namespace Helios.LikeARogue.Components
{
    public class InputComponent
    {
        public Key KeyPress { get; set; }
        public bool WasKeyPressed { get; set; }
        //public float ElapsedTimeSinceKeypress
    }
}
