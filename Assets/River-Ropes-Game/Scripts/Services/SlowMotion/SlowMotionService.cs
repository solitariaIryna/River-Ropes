using Cysharp.Threading.Tasks;
using RiverRopes.Constants;
using System;
using UnityEngine;

namespace RiverRopes.Services.SlowMotion
{
    public class SlowMotionService
    {
        private float _fixedTimeStep = 0.02f;

        private SlowmotionPreset _currentPreset;
        private bool isSlowmotionRunning;

        private SlowMotionService()
        {
            _fixedTimeStep = Time.fixedDeltaTime;
            SetDefaultTimeScale();
        }

        public void DoSlowmotion(SlowmotionPreset preset, Action onCompleted = null)
        {
            _currentPreset = preset;
            StopSlowmotionInstant();
            _ = DoSlowmotionRoutine(preset, onCompleted);
        }

        public void StopSlowmotionSmoothly()
        {
            if (isSlowmotionRunning)
            {
                isSlowmotionRunning = false;
                _ = ChangeTimeScaleRoutine(_currentPreset.Factor, GameConstants.TIME_SCALE, _currentPreset.LeaveTime, _currentPreset.LeaveCurve);
            }
        }

        public void StopSlowmotionInstant()
        {
            isSlowmotionRunning = false;
            SetDefaultTimeScale();
        }

        private void SetDefaultTimeScale()
        {
            SetTimeScale(GameConstants.TIME_SCALE);
        }

        private void SetTimeScale(float newTimeScale)
        {
            Time.timeScale = newTimeScale;
            Time.fixedDeltaTime = Time.timeScale * _fixedTimeStep;
        }

        private async UniTask DoSlowmotionRoutine(SlowmotionPreset preset, Action onCompleted)
        {
            isSlowmotionRunning = true;

            await ChangeTimeScaleRoutine(GameConstants.TIME_SCALE, preset.Factor, preset.EnterTime, preset.EnterCurve);

            if (isSlowmotionRunning)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(preset.Duration), DelayType.Realtime);
            }

            if (isSlowmotionRunning)
            {
                await ChangeTimeScaleRoutine(preset.Factor, GameConstants.TIME_SCALE, preset.LeaveTime, preset.LeaveCurve);
                onCompleted?.Invoke();
            }

            isSlowmotionRunning = false;
        }

        private async UniTask ChangeTimeScaleRoutine(float from, float to, float changeTime, AnimationCurve curve)
        {
            float percent = 0f;
            float speed = 1f / changeTime;

            while (percent < 1f && isSlowmotionRunning)
            {
                percent += speed * Time.unscaledDeltaTime;
                float newTimeScale = Mathf.Lerp(from, to, curve.Evaluate(percent));
                SetTimeScale(newTimeScale);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            SetTimeScale(to);
        }
    }
}
