using UnityEngine;
using UnityEngine.UI;

namespace VitsehLand.Scripts.UI.MonitorGarden
{
    public abstract class SlotState : MonoBehaviour
    {
        [Header("UI Element")]
        public Image slotPanel;
        public Image lockImage;

        public Color lockPanelColor;
        public Color unlockPanelColor;

        public bool isUnlock;
        public int index;

        public virtual void ChangeLockState(bool _isUnlock)
        {
            isUnlock = _isUnlock;
            slotPanel.color = isUnlock ? unlockPanelColor : lockPanelColor;
            lockImage.gameObject.SetActive(!isUnlock);
        }
    }
}