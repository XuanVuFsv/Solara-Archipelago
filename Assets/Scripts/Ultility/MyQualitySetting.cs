using TMPro;
using UnityEngine;
using VitsehLand.Scripts.Pattern.Singleton;

namespace VitsehLand.Scripts.Ultility
{
    public class MyQualitySetting : Singleton<MyQualitySetting>
    {
        [SerializeField]
        TextMeshProUGUI vSyncText;

        public override void Awake()
        {
            base.Awake();

            vSyncText.text = QualitySettings.vSyncCount.ToString();
            QualitySettings.vSyncCount = 0;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                if (QualitySettings.vSyncCount == 3) QualitySettings.vSyncCount = 0;
                else QualitySettings.vSyncCount++;
                vSyncText.text = QualitySettings.vSyncCount.ToString();
            }

            //if (Input.GetKeyDown(KeyCode.F)) Application.targetFrameRate = fps;

            //if (Input.GetKeyDown(KeyCode.L))
            //{
            //    Cursor.visible = false;
            //    Cursor.lockState = CursorLockMode.Locked;
            //}

            //if (Input.GetKeyDown(KeyCode.U))
            //{
            //    Cursor.visible = true;
            //    Cursor.lockState = CursorLockMode.None;
            //}
        }
    }
}