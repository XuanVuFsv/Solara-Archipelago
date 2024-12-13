using TMPro;
using UnityEngine;

namespace VitsehLand.Scripts.UI.Weapon
{
    public class WeaponUI : MonoBehaviour
    {
        public TMP_Text weaponName;
        public RectTransform panel;

        // Update is called once per frame
        void Update()
        {
            //Make weapon UI on the ground can be seen by the player in every direction
            transform.LookAt(Camera.main.transform.position);
        }
    }
}