
public abstract class ObjectState
{
    protected Suckable objectMachine;
    
    public ObjectState(Suckable sucked)
    {
        objectMachine = sucked;
    }

    public ObjectState()
    {
        objectMachine = null;
    }

    public virtual void Start()
    {

    }

    public virtual void End()
    {

    }

    public virtual void ResetCropStats()
    {

    }
}