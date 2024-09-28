using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Suckable : StateMachine, ISuckable
{
    //public enum SuckableObjectType
    //{
    //    Plant = 0,
    //    PowerContainer = 1,
    //    NaturalResource = 2,
    //}

    //protected SuckableObjectType type;

    public CropStats cropStats;
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
  
    public CropStats GetCropStats()
    {
        return cropStats;
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
        Debug.Log("Pool" + cropStats.name + "Setup");
        Debug.Log("Add Game Event Pool" + cropStats.name + "Setup");

        PoolingManager.Instance.AddGameEvent("Pool" + cropStats.name + "Setup");
    }

    public virtual void RemoveUseGameEvent()
    {
        PoolingManager.Instance.RemoveGameEvent("Pool" + cropStats.name + "Setup");
        Debug.Log("Remove Game Event Pool" + cropStats.name + "Setup");
    }
}
