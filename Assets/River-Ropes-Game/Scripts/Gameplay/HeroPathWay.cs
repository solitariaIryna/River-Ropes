using UnityEngine;
using UnityEngine.Splines;

namespace RiverRopes.Gameplay
{
    public class HeroPathWay : MonoBehaviour
    {
        [SerializeField] private SplineContainer _spawnTrajectory;

        public Vector3 EvaluatePosition(float t) => transform.TransformPoint(_spawnTrajectory.Spline.EvaluatePosition(t));
        public Vector3 EvaluateTangent(float t) => _spawnTrajectory.Spline.EvaluateTangent(t);

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private bool _drawPointsDebug = true;
        [SerializeField] private Color _sphereColor = Color.yellow;
        [SerializeField] private float _sphereRadius = 0.5f;

        private void OnDrawGizmosSelected()
        {
            if (_spawnTrajectory == null || _drawPointsDebug == false)
                return;

            Spline spline = _spawnTrajectory.Spline;
            Gizmos.color = _sphereColor;

            for (int i = 0; i < spline.Count; i++)
            {
                Vector3 localPosition = _spawnTrajectory.Spline[i].Position;
                Vector3 worldPosition = transform.TransformPoint(localPosition);
                Gizmos.DrawSphere(worldPosition, _sphereRadius);
            }
        }
#endif

    }

}
