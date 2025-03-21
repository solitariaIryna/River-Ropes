using UnityEngine;

namespace RiverRopes.Gameplay
{
    public class SpawnPoint : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private bool _drawDebug = true;
        [SerializeField] private Color _sphereColor = Color.red;
        [SerializeField] private float _sphereRadius = 0.5f;

        private void OnDrawGizmos()
        {
            if (_drawDebug == false)
                return;

            Gizmos.color = _sphereColor;

            Vector3 direction = transform.forward * 2f;
            Vector3 targetPosition = transform.position + direction;

            Gizmos.DrawSphere(transform.position, _sphereRadius);
            
        }
#endif
    }

}
