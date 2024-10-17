using UnityEngine;
using VitsehLand.Assets.Scripts.Interactive;

namespace VitsehLand.Scripts.UI.Interactive
{
    public class SimpleShowGuide : ActivateBehaviour
    {
        public GameObject guildPanel;

        public override void ExecuteActivateAction()
        {
            guildPanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void ClosePanel()
        {
            guildPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}