
public interface ISuckable
{
    public void GoToCollector();
    public void ChangeToStored();
    public void ChangeToUnStored();
    public CropStats GetCropStats();
    public int GetCropContain();
    public void SetCropContain(int count);
}
