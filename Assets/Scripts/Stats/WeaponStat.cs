using UnityEngine;
using VitsehLand.Scripts.Weapon.General;

namespace VitsehLand.Scripts.Stats
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
    public class WeaponStat : ScriptableObject
    {
        public new string name;
        public float runSpeed;
        public Sprite artwork;
        public ActiveWeapon.WeaponSlot weaponSlot;

        //public int cost;
        //public float penetrattionThickness;
        //public int damageHead, damageBody, damageArmsLegs;
        //public int dropOffDsitance;
        //public int decreseDamageRate;
        //public float fireRate;
        //public float reloadSpeed;
        //public int magazine;
        //public List<Vector2> recoildPattern;
        //public Transform casingPrefab;
        //public TrailRenderer bulletTracer;
        //public ParticleSystem hitEffectPrefab;
        public AnimationClip weaponAnimation;
    }
}