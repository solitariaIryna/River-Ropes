using RiverRopes.Gameplay.Entities;
using System;

namespace RiverRopes.Gameplay.Levels
{
    [Serializable]
    public class AttackSettings
    {
        public Entity Target;
        public AttackCameraSettings CameraSettings;
    }
}
