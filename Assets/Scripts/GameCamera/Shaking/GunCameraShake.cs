using UnityEngine;
using Cinemachine;

namespace VitsehLand.Assets.Scripts.GameCamera.Shaking
{
    public class GunCameraShake : MonoBehaviour
    {
        public CinemachineImpulseSource gunCameraShake;

        public void Awake()
        {
            gunCameraShake = GetComponent<CinemachineImpulseSource>();
        }

        public void GenerateRecoil()
        {
            gunCameraShake.GenerateImpulse(Camera.main.transform.forward);
        }
    }
}