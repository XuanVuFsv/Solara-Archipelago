using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public ObjectState state;

    public void EndCurrentState()
    {
        if (state == null) return;
        state.End();
    }

    public void SetState(ObjectState state)
    {
        this.state = state;
        this.state.Start();
    }
}
