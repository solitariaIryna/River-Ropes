using RiverRopes.Gameplay.Entities;
using System;

namespace RiverRopes.Gameplay.Levels
{
    [Serializable]
    public struct AttackSettings
    {
        public Entity Target;
        public AttackCameraSettings CameraSettings;
    }
}
