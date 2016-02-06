using System.Collections.Generic;

namespace Helios.Core
{
    public abstract class Subsystem
    {
		protected List<uint> RelevantEntities;
        private List<uint> _entitiesToAdd;
        private List<uint> _entitiesToRemove;
          
        public BitSet ComponentMask { get; private set; }
        /// <summary>
        /// The world that the subsystem operates in.
        /// </summary>
        protected World World { get; private set; }
       
        protected Subsystem(World theWorld)
        {
            World = theWorld;
            World.EntityManager.RegisterSubsystem(this);
            ComponentMask = new BitSet();
            RelevantEntities = new List<uint> ();
            _entitiesToAdd = new List<uint>();
            _entitiesToRemove = new List<uint>();

        }

        /// <summary>
        /// Update all entities in the world whose component masks include the component mask of this subsystem.
        /// </summary>
        /// <param name="dt"></param>
        public virtual void Update(float dt)
        {
            if (_entitiesToAdd.Count > 0)
            {
                RelevantEntities.AddRange(_entitiesToAdd);
                _entitiesToAdd.Clear();
            }

            if (_entitiesToRemove.Count > 0)
            {
                RelevantEntities.RemoveAll(x => _entitiesToRemove.Contains(x));
                _entitiesToRemove.Clear();
            }
        }

		public virtual void AddEntity(uint entity)
		{
			if (!RelevantEntities.Contains (entity))
                _entitiesToAdd.Add (entity);
		}

		public virtual void RemoveEntity(uint entity)
		{
			if (RelevantEntities.Contains (entity))
                _entitiesToRemove.Add (entity);
		}

		public bool HasEntity(uint entity)
		{
			return RelevantEntities.IndexOf(entity) != -1 || _entitiesToAdd.IndexOf(entity) != -1 || _entitiesToRemove.IndexOf(entity) != -1;
		}

    }
}
