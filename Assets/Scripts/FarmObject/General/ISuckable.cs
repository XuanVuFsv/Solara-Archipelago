using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISuckable
{
    public void GoToAxieCollector();
    public void ChangeToStored();
    public void ChangeToUnStored();
    public AmmoStats GetAmmoStats();
    public int GetAmmoContain();
    public void SetAmmoContain(int count);
}
