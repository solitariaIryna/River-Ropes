using UnityEngine;

namespace RiverRopes.Gameplay.Entities
{
    public enum EntityType
    {
        Boat,
        Rope,
        Enemy,
        Trash
    }
    public abstract class Entity : MonoBehaviour
    {
        
    }
    public class Boat : Entity
    {

    }
    public class Rope : Entity
    {

    }
    public class ShootTrigger : MonoBehaviour
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
