
public abstract class CropState
{
    protected Plant cropMachine;
    
    public CropState(Plant plant)
    {
        cropMachine = plant;
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