using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrowingResourceManager : ActivateBehaviour
{
    public int fullResourceValue = 100;
    [SerializeField]
    protected int currentResourceValue = 0;
    public int CurrentResourceValue
    {
        get { return currentResourceValue; }
    }

    public int timeToUseAllResource;
    public int refillMaxFee;
    public bool outOfResource = true;
    public bool inRefill = false;

    protected float t = 0;

    // Update is called once per frame
    void Update()
    {
        if (!outOfResource && !inRefill)
        {
            t += Time.deltaTime;
            currentResourceValue = (int)Mathf.Lerp(fullResourceValue, 0, t / timeToUseAllResource);
            if (currentResourceValue <= 0) StopUseResource();
        }
    }

    public override void ExecuteActivateAction()
    {
        RefillResource();
    }

    public virtual void StopUseResource()
    {
        outOfResource = true;
    }

    public virtual bool RefillResource()
    {
        bool canRefill = GemManager.Instance.UseGem((int)((1 - GetCurrentResourceValueRatio()) * refillMaxFee));
        Debug.Log((int)((1 - GetCurrentResourceValueRatio()) * refillMaxFee));
        if (!canRefill) return false;

        inRefill = true;

        currentResourceValue = fullResourceValue;
        // Do something when the tween is complete
        inRefill = false;
        outOfResource = false;
        t = 0;

        return true;
    }

    public float GetCurrentResourceValueRatio()
    {
        return (float)((float)currentResourceValue / (float)fullResourceValue);
    }
}