using UnityEngine;

namespace VitsehLand.Scripts.UI.MonitorGarden
{
    public abstract class SlotManager : MonoBehaviour
    {
        public string resourceName;
        public int maxStackPerSlot;
        public Color defaultLockPanelColor, defaultUnlockPanelColor;
    }
}