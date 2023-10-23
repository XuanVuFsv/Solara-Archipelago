using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo", menuName = "Ammo")]
public class AmmoStats : ScriptableObject
{
    [Header("Type of fruit")]
    [Tooltip("Fruit can attack or not")]
    public bool canAttack;

    [Tooltip("Slot for fruit. Example slot 1 is attack fruit, while slot 3 is normal fruit")]
    public ActiveWeapon.WeaponSlot weaponSlot;

    public new string name;
    public int maxCount;
    public int ammoAllowedInMagazine;
    public int amplitudeGainImpulse;
    public float fireRate;

    [Tooltip("Type of fruit. Now, it's can be same as fruit name")]
    public enum FruitType
    {
        Null = -1,
        Berry = 0,
        Chilli = 1,
        Onion = 2,
        Punch = 4,
        Star = 5,
        Tomato = 6,
        Waternelon = 7
    }
    public FruitType fruitType;

    public Sprite artwork;

    public enum ShootingHandleType
    {
        None = -1,
        Raycast = 0,
        InstantiateBullet = 1
    }
    public ShootingHandleType shootingHandleType;

    public GameObject trailTracer;
    public GameObject bulletObject;
    public ParticleSystem hitEffectPrefab;

    public GameObject GetTrailTracer()
    {
        return trailTracer;
    }

    [Header("Attacking Fruit")]
    //public int damageHead, damageBody, damageArmsLegs;
    //public int dropOffDsitance;
    //public int decreseDamageRate;
    public int damage;
    [Tooltip("Only for attacking fruit. With Specific Enemy. This fruit will deal more damage")]
    public int damageDealToSpecificEnemy;

    [Tooltip("Only for attack fruit. Scope mode of fruit")]
    public enum ZoomType
    {
        NoZoom = 0,
        CanZoom = 1,
        HasScope = 2
    }
    public ZoomType zoomType;
    [Tooltip("Only for attacking fruit. What effect will be ins when shooting fruit")]
    public enum BulletEffectComponent
    {
        None = -1,
        PlaceHole = 0,
        InstantiateTrail = 1,
        Both = 2
    }
    public BulletEffectComponent bulletEffectComponent;

    [Tooltip("Only for attacking fruit. Range for raycast checking")]
    public int range;
    [Tooltip("Only for attacking fruit and InstantiateBullet")]
    public int force;

    //public float runSpeed;
    public float reloadSpeed;
    public float multiplierRecoilOnAim;
    public float multiplierForAmmo;
    public List<Vector2> recoildPattern;

    [Tooltip("number of bullet or raycast will be instantiated when player shoot. Example 1 for rifle type (berry), sniper type(star) and 5 for shotgun type(tomato)")]
    public int bulletCount = 1;
    public List<Vector3> bulletDirectionPattern;

    [Header("Normal Fruit")]
    public int growingTime;
    public int numberOfFruitPerSeed;

    public enum BodyType
    {
        Tree = 0,
        Vegetable = 1,
        Climbing = 2
    }
    public BodyType bodyType;
    public int wateringTime;
    public int requiredLevel;
    public GameObject growingBody;
    public GameObject prefab;
}
