using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Suckable : StateMachine, ISuckable
{
    public AmmoStats ammoStats;
    public int ammoContain;

    public Rigidbody rigid;
    public Collider suckableCollider;

    float s;
    float varSpeed;

    public void GoToAxieCollector()
    {
        s += Time.deltaTime * CollectHandler.Instance.acceleratonSuckUpSpeed;
        varSpeed = Mathf.Lerp(CollectHandler.Instance.minSuckUpSpeed, CollectHandler.Instance.maxSuckUpSpeed, CollectHandler.Instance.velocityCurve.Evaluate(s / 1f));
        
        transform.position = Vector3.Lerp(transform.position, CollectHandler.Instance.shootingInputData.raycastOrigin.position, varSpeed);
    }

    public void MoveOut()
    {
        rigid.AddForce(CollectHandler.Instance.shootingInputData.bulletSpawnPoint.forward * CollectHandler.Instance.moveOutForce);
    }

    public void ResetVelocity()
    {
        rigid.velocity = Vector3.zero;
    }
  
    public AmmoStats GetAmmoStats()
    {
        return ammoStats;
    }
    
    public int GetAmmoContain()
    {
        return ammoContain;
    }

    public void SetAmmoContain(int count)
    {
        ammoContain = count;
    }

    public virtual void ChangeToStored()
    {

    }

    public virtual void ChangeToSeed()
    {

    }

    public virtual void AddUsedGameEvent()
    {
        Debug.Log("Pool" + ammoStats.name + "Setup");
        Debug.Log("Add Game Event Pool" + ammoStats.name + "Setup");

        PoolingManager.Instance.AddGameEvent("Pool" + ammoStats.name + "Setup");
    }

    public virtual void RemoveUseGameEvent()
    {
        PoolingManager.Instance.RemoveGameEvent("Pool" + ammoStats.name + "Setup");
        Debug.Log("Remove Game Event Pool" + ammoStats.name + "Setup");
    }
}
