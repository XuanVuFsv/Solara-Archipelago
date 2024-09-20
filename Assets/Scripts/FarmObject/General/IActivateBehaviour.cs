using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivateBehaviour
{
    public void ExecuteActivateAction();
}

public abstract class ActivateBehaviour: MonoBehaviour, IActivateBehaviour
{
    public virtual void ExecuteActivateAction()
    {

    }
}