using UnityEngine;
using VitsehLand.GameCamera.Shaking;
using VitsehLand.Scripts.Pattern.Observer;
using VitsehLand.Scripts.Player;
using VitsehLand.Scripts.Stats;
using VitsehLand.Scripts.Weapon.Ammo;

namespace VitsehLand.Scripts.Weapon.Primary
{
    [System.Serializable]
    public class ShootingInputData
    {
        public ShootingInputData(ShootController _shootController, CropStats.ShootingHandleType _shootingHandleType, CropStatsController _cropStatsController, AudioSource _source, Transform _raycastOrigin, Transform _fpsCameraTransform, GameEvent _hitEvent, CameraShake _cameraShake, Transform _bulletSpawnPoint, int _layerMask)
        {
            shootController = _shootController;
            shootingHandleType = _shootingHandleType;
            cropStatsController = _cropStatsController;
            source = _source;
            raycastOrigin = _raycastOrigin;
            fpsCameraTransform = _fpsCameraTransform;
            hitEvent = _hitEvent;
            cameraShake = _cameraShake;
            bulletSpawnPoint = _bulletSpawnPoint;
            layerMask = _layerMask;
        }

        public ShootController shootController;

        public CropStats.ShootingHandleType shootingHandleType;
        public CropStatsController cropStatsController;
        public AudioSource source;
        public Transform raycastOrigin;
        public Transform fpsCameraTransform;
        public GameEvent hitEvent;
        public CameraShake cameraShake;
        public Transform bulletSpawnPoint;
        public int layerMask;
    }
}