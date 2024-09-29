using UnityEngine;
using VitsehLand.Scripts.Farming.General;

namespace VitsehLand.Scripts.Weapon.Ammo
{
    public class AmmoPickup : Suckable
    {
        public Crop.PlantState plantState;
        public Suckable suckableSample;
        // Start is called before the first frame update
        void Start()
        {
            rigid = GetComponent<Rigidbody>();
            suckableCollider = GetComponent<Collider>();
        }
    }
}