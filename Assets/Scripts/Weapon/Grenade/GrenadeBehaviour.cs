using UnityEngine;

namespace VitsehLand.Scripts.Weapon.Grenade
{
    public class GrenadeBehaviour : MonoBehaviour
    {
        public Rigidbody rigid;

        // Start is called before the first frame update
        void Start()
        {
            if (rigid == null) rigid = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddForce()
        {

        }
    }
}