using UnityEngine;

namespace RiverRopes.Gameplay.Entities
{
    public class Hero : Entity
    {
        private HeroPathWay _pathWay;
        public float _speed = 0.5f; 
        public float _rotationSpeed = 5f; 
        public float _bobbingAmplitude = 0.1f; 
        public float _driftAmount = 2f; 
        public float _waveFrequency = 0.5f; 

        private float _timeOffset; 
        private float _t;

        public void SetMovePath(HeroPathWay riverWay)
        {
            _pathWay = riverWay;
        }

        public void Initialize()
        {

        }

        private void Update()
        {
            _t += 0.02f * Time.deltaTime;
            if (_t > 1f) _t = 0f; 

            Vector3 position = _pathWay.EvaluatePosition(_t);
            float bobbing = Mathf.Sin(_t * _waveFrequency + _timeOffset) * _bobbingAmplitude;
            position.y += bobbing;

            Vector3 tangent = _pathWay.EvaluateTangent(_t);

            Vector3 drift = new Vector3(
                Mathf.Sin(Time.time * _driftAmount) * 0.1f, 
                0f,
                Mathf.Cos(Time.time * _driftAmount) * 0.1f  
            );

            position += drift;

            Quaternion targetRotation = Quaternion.LookRotation(tangent);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            transform.position = position;
        }
    }
}
