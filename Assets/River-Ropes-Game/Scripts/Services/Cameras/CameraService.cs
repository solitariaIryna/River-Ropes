using RiverRopes.Services.AssetProvider;
using RiverRopes.Services.ConfigsProvider;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace RiverRopes.Services.Cameras
{
    public class CameraService
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IConfigsProvider _configsProvider;
        private Dictionary<CameraType, CinemachineCamera> _cameras = new();
        private CinemachineCamera _camera;
        private Transform _target;

        private const string CAMERAS_PARENT_NAME = "Cameras";

        private bool _hasCamera => _camera != null;
        private CameraService(IAssetProvider assetProvider, IConfigsProvider configsProvider)
        {
            _assetProvider = assetProvider;
            _configsProvider = configsProvider;
        }
        public void CreateCameras()
        {
            Transform camerasParent = new GameObject(CAMERAS_PARENT_NAME).transform;

            CameraStorage storage = _configsProvider.Cameras;

            foreach (CameraType type in storage.Cameras.Keys)
            {
                CinemachineCamera camera = _assetProvider.Instantiate<CinemachineCamera>(storage.Cameras[type]);
                camera.transform.parent = camerasParent;
                _cameras[type] = camera;
                camera.enabled = false;
            }
        }
        public void TurnOn(CameraType type)
        {
            if (_hasCamera)
                _camera.enabled = false;

            _camera = _cameras[type]; 

            _camera.Target.TrackingTarget = _target;
            _camera.enabled = true;            
        }
        public void SetFollowTarget(Transform target)
        {
            _target = target;
        }
        public void SetCameraFollowTarget(Transform target)
        {
            _camera.Target.TrackingTarget = target;
        }

    }
}
