namespace RiverRopes.Gameplay.Entities
{
    public enum EntityType
    {
        Boat,
        Rope,
        Enemy,
        Trash
    }
    public abstract class Entity
    {
        
    }
    public class Boat : Entity
    {

    }
    public class Rope : Entity
    {

    }
    public class Enemy : Entity
    {

    }
    public class Trash : Entity
    {

    }
    public class EntityData
    {
        public int UniqueId;
    }
}
