using RiverRopes.Services.Cameras;
using RiverRopes.Services.SlowMotion;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using CameraType = RiverRopes.Services.Cameras.CameraType;

namespace RiverRopes.Gameplay.Levels
{
    public class AttackSlowMotionTriggerZone : TriggerZone
    {
        [SerializeField] private List<AttackSettings> _attackQueue;
        [Space]
        [SerializeField] private SlowmotionPreset _slowmotionPreset;

        private CameraService _cameraService;
        private SlowMotionService _slowMotionService;

        private AttackSettings _currentSettings;

        [Inject]
        public void Construct(CameraService cameraService, SlowMotionService slowMotionService)
        {
            _cameraService = cameraService;
            _slowMotionService = slowMotionService;
        }
        protected override void OnTriggerEnter(Collider other)
        {
            _slowMotionService.DoSlowmotion(_slowmotionPreset);
            _cameraService.TurnOn(CameraType.Attack);
            _cameraService.SetCameraFollowTarget(_attackQueue[0].Target.transform);
        }
    }

}
