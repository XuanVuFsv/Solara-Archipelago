using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suckable : MonoBehaviour, ISuckable
{
    [SerializeField]
    private AmmoStats ammoStats;
    [SerializeField]
    private int ammoContain;

    public Rigidbody rigid;

    float s;
    float varSpeed;

    public void GoToAxieCollector()
    {
        Vector3 dir = CollectHandler.Instance.transform.position - transform.position;

        s += Time.deltaTime * CollectHandler.Instance.acceleratonSuckUpSpeed;
        varSpeed = Mathf.Lerp(CollectHandler.Instance.minSuckUpSpeed, CollectHandler.Instance.maxSuckUpSpeed, CollectHandler.Instance.velocityCurve.Evaluate(s / 1f));
        rigid.velocity = dir.normalized * varSpeed;
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
}
