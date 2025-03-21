using RiverRopes.Services.SlowMotion;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RiverRopes.Gameplay.Levels
{
    public class AttackSlowMotionTriggerZone : TriggerZone
    {
        [SerializeField] private List<AttackSettings> _attackQueue;
        [Space]
        [SerializeField] private SlowmotionPreset _slowmotionPreset;

        private SlowMotionService _slowMotionService;

        [Inject]
        public void Construct(SlowMotionService slowMotionService)
        {
            _slowMotionService = slowMotionService;
        }
        protected override void OnTriggerEnter(Collider other)
        {
            _slowMotionService.DoSlowmotion(_slowmotionPreset);
        }
    }
}
