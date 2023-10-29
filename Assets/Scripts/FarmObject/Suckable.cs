using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suckable : MonoBehaviour, ISuckable
{
    public enum PlantState
    {
        Seed = 0,
        Stored = 1,
        GrowingBody = 2,
        Ripe = 3
    }

    public AmmoStats ammoStats;
    [SerializeField]
    public int ammoContain;

    public Rigidbody rigid;
    public Collider collider;

    float s;
    float varSpeed;

    public void GoToAxieCollector()
    {
        //Vector3 dir = CollectHandler.Instance.shootingInputData.raycastOrigin.position - transform.position;

        s += Time.deltaTime * CollectHandler.Instance.acceleratonSuckUpSpeed;
        varSpeed = Mathf.Lerp(CollectHandler.Instance.minSuckUpSpeed, CollectHandler.Instance.maxSuckUpSpeed, CollectHandler.Instance.velocityCurve.Evaluate(s / 1f));
        //rigid.velocity = dir.normalized * varSpeed;
        
        transform.position = Vector3.Lerp(transform.position, CollectHandler.Instance.shootingInputData.raycastOrigin.position, varSpeed);
    }

    public void ResetVelocity()
    {
        rigid.velocity = Vector3.zero;
    }

    public virtual void ChangeToStored()
    {

    }

    public virtual void ChangeToSeed()
    {

    }

    public void MoveOut()
    {
        rigid.AddForce(CollectHandler.Instance.shootingInputData.bulletSpawnPoint.forward * CollectHandler.Instance.moveOutForce);
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

    public virtual void AttachAmmoToObject(Transform parent, bool isVisible)
    {
        if (!parent.GetComponent<WeaponStatsController>().ofActiveAmmo) return;
        PoolingManager.Instance.AddGameEvent("Pool" + ammoStats.name + "Setup");
        //Debug.Log("Add Game Event Pool" + ammoStats.name + "Setup");
    }

    public virtual void DetachAmmoToObject()
    {
        PoolingManager.Instance.RemoveGameEvent("Pool" + ammoStats.name + "Setup");
    }
}
