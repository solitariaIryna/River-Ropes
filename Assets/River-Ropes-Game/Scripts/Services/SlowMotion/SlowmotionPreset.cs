using System;
using UnityEngine;

namespace RiverRopes.Services.SlowMotion
{
    [CreateAssetMenu(fileName = nameof(SlowmotionPreset), menuName = "Configs/SlowMotion/SlowmotionPreset")]
    public class SlowmotionPreset : ScriptableObject
    {
        [Range(0f, 1f)]
        public float Factor = 0.1f;
        public float Duration = 1f;
        public float EnterTime = 0.25f;
        public float LeaveTime = 0.25f;
        public AnimationCurve EnterCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public AnimationCurve LeaveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }
}
