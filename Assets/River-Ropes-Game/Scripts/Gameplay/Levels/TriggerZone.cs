using UnityEngine;

namespace RiverRopes.Gameplay.Levels
{
    public abstract class TriggerZone : MonoBehaviour
    {
        protected abstract void OnTriggerEnter(Collider other);
    }
}
