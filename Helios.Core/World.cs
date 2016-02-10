namespace Helios.Core
{
    /// <summary>
    /// Base class that houses all entities.  Inherited objects should contain one array for each component type 
    /// initialized to the length of MaxEntities.  All subsystems live here as well and get called to update and draw.
    /// Includes base methods for creation and destruction of entities.
    /// 
    /// </summary>
    public abstract class World
    {
		public EntityManager EntityManager { get; private set; }

		protected World ()
		{
			EntityManager = new EntityManager(this);
		}

        /// <summary>
        /// Update all subsystems.
        /// </summary>
        /// <param name="dt"></param>
        public abstract void Update(float dt);

        /// <summary>
        /// Render any subsystem output to the screen.
        /// </summary>
        public abstract void Render();

    }
}
