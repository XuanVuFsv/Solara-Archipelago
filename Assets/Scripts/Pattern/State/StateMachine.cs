using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public CropState state;

    public void EndCurrentState()
    {
        if (state == null) return;
        state.End();
    }

    public void SetState(CropState state)
    {
        this.state = state;
        this.state.Start();
    }
}
