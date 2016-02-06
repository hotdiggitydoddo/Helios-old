﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Helios.LikeARogue
{
    public abstract class Game
    {
        private Clock _clock;
        public RenderWindow Window;
        protected Color clearColor;

        public Game(uint width, uint height, string title, Color clearColor)
        {
            this.Window = new RenderWindow(new VideoMode(width, height), title, Styles.Close);
            this.clearColor = clearColor;

            // Set up events
            Window.Closed += OnClosed;
            _clock = new Clock();
        }

        public void Run()
        {
            LoadContent();
            Initialize();
            
            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                var elapsed = _clock.Restart().AsSeconds();
                Tick(elapsed);

                Window.Clear(clearColor);
                Render();
                Window.Display();
            }
        }

        protected abstract void LoadContent();
        protected abstract void Initialize();

        protected abstract void Tick(float elapsed);
        protected abstract void Render();

        private void OnClosed(object sender, EventArgs e)
        {
            Window.Close();
        }
    }
}
