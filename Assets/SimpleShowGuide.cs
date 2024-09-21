using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleShowGuide : ActivateBehaviour
{
    public GameObject guildPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
