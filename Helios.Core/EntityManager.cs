using System.Collections.Generic;

namespace Helios.Core
{
	public class EntityManager
	{
	    private readonly Dictionary<uint, BitSet> _entities; 
	    private readonly List<Subsystem> _subsystems;
	    private static uint _nextId;
        public World World { get; private set; }

		public EntityManager (World world)
		{
			World = world;
			_entities = new Dictionary<uint, BitSet>();
			_subsystems = new List<Subsystem>();
		}

		public void RegisterSubsystem(Subsystem subsystem)
		{
		    _subsystems.Add(subsystem);
		}

	    public uint CreateEntity()
	    {
	        var entity = _nextId;

            _entities.Add(entity, new BitSet());
	        _nextId++;

            return entity;
	    }

	    public void DestroyEntity(uint entity)
	    {
            if (!_entities.ContainsKey(entity)) return;

	        foreach (var subsystem in _subsystems)
	        {
	            if (subsystem.HasEntity(entity))
                    subsystem.RemoveEntity(entity);
	        }

	        _entities.Remove(entity);
	    }

		public void AddComponent(uint entity, int componentType)
		{
		    if (!_entities.ContainsKey(entity)) return;

            // if the entity already has the component, exit
			if (_entities [entity].IsSet(componentType)) return;

            // set the component bit
			_entities [entity].SetBit(componentType);

            // tell all systems that now match this entities bitmask to add the entity to their internal update lists
			foreach (var subsystem in _subsystems)
				if (!subsystem.HasEntity(entity) && subsystem.ComponentMask.IsSubsetOf (_entities [entity]))
					subsystem.AddEntity (entity);
		}

	    public bool HasComponent(int componentType, uint entity)
	    {
	        return _entities[entity].IsSet(componentType);
	    }

		public void RemoveComponent(uint entity, int componentType)
		{
		    if (!_entities.ContainsKey(entity)) return;

            // if the entity doesn't have this component, exit
			if (!_entities [entity].IsSet(componentType)) return;

            // clear the component type's bit 
			_entities [entity].ClearBit (componentType);

            // tell all systems which are tracking the entity that the bitmask changed and to drop the entity from
            // their list if the entity no longer matches their required component set.
		    foreach (var subsystemList in _subsystems)
		    {
		        var subsystemsWithEntity = _subsystems.FindAll(x => x.HasEntity(entity));
		        foreach (var subsystem in subsystemsWithEntity)
		        {
		            if (subsystem.HasEntity(entity) && !subsystem.ComponentMask.IsSubsetOf(_entities[entity]))
                        subsystem.RemoveEntity(entity);
		        }
		    }
		}
	}
}

