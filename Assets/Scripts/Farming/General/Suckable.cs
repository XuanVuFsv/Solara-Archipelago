using UnityEngine;
using VitsehLand.Assets.Scripts.Weapon.Collector;
using VitsehLand.Scripts.Pattern.Pooling;
using VitsehLand.Scripts.Pattern.State;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.Farming.General
{
    public abstract class Suckable : StateMachine, ISuckable
    {
        public CollectableObjectStat collectableObjectStat;
        public int cropContain;

        public Rigidbody rigid;
        public Collider suckableCollider;

        [SerializeField]
        protected float s;
        [SerializeField]
        protected float varSpeed;

        public virtual void GoToCollector()
        {
            s += Time.deltaTime * CollectHandler.Instance.acceleratonSuckUpSpeed;
            varSpeed = Mathf.Lerp(CollectHandler.Instance.minSuckUpSpeed, CollectHandler.Instance.maxSuckUpSpeed, CollectHandler.Instance.velocityCurve.Evaluate(s / 1f));

            transform.position = Vector3.Lerp(transform.position, CollectHandler.Instance.shootingInputData.raycastOrigin.position, varSpeed);
        }

        public virtual void MoveOut()
        {
            rigid.AddForce(CollectHandler.Instance.shootingInputData.bulletSpawnPoint.forward * CollectHandler.Instance.moveOutForce);
        }

        public virtual void ResetVelocity()
        {
            rigid.velocity = Vector3.zero;
        }

        public CollectableObjectStat GetCollectableObjectStat()
        {
            return collectableObjectStat;
        }

        public int GetCropContain()
        {
            return cropContain;
        }

        public void SetCropContain(int count)
        {
            cropContain = count;
        }

        public virtual void ChangeToStored()
        {

        }

        public virtual void ChangeToUnStored()
        {

        }

        public virtual void AddUsedGameEvent()
        {
            Debug.Log("Pool" + collectableObjectStat.collectableObjectName + "Setup");
            Debug.Log("Add Game Event Pool" + collectableObjectStat.collectableObjectName + "Setup");

            PoolingManager.Instance.AddGameEvent("Pool" + collectableObjectStat.collectableObjectName + "Setup");
        }

        public virtual void RemoveUseGameEvent()
        {
            PoolingManager.Instance.RemoveGameEvent("Pool" + collectableObjectStat.collectableObjectName + "Setup");
            Debug.Log("Remove Game Event Pool" + collectableObjectStat.collectableObjectName + "Setup");
        }
    }
}