using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NaturalResource : Suckable
{
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        suckableCollider = GetComponent<Collider>();

        SetState(new ResourceUnStored(this));
    }

    public override void ChangeToStored()
    {
        EndCurrentState();
        SetState(new ResourceStored(this));
    }

    public override void ChangeToUnStored()
    {
        EndCurrentState();
        SetState(new ResourceUnStored(this));
    }
}
