using UnityEngine;
using VitsehLand.Scripts.Pattern.Pooling;

namespace VitsehLand.Scripts.Weapon.Ammo
{
    public class BulletHoleBehaviour : ObjectInPool
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }

        public override void OnUsed(RaycastHit hit)
        {
            //MyDebug.Log(gameObject.name);
            gameObject.SetActive(true);

            transform.position = hit.point;
            transform.forward = hit.normal;
            transform.rotation = Quaternion.LookRotation(hit.normal);
            Invoke(nameof(Release), lifeTime);
        }

        public override void OnUsed(Vector3 point, Vector3 normal)
        {
            //MyDebug.Log(gameObject.name);
            gameObject.SetActive(true);

            transform.position = point;
            transform.forward = normal;
            transform.rotation = Quaternion.LookRotation(normal);
            Invoke(nameof(Release), lifeTime);
        }
    }
}