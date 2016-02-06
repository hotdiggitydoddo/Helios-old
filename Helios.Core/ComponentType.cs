namespace Helios.Core
{
    /// <summary>
    /// Integer definition of all the different types of components in a game. 
    /// Entity component masks are comprised of the component types to tell subsystems what kinds
    /// of components the entity has.  Subsystem component masks are comprised of the component types to
    /// define which set of components are required to be present on any entity for the subsystem to operate on it. 
    /// </summary>
    public abstract class ComponentType
    {
        public const int NoComponents = 0;
    }

	public interface IComponent { }
}
