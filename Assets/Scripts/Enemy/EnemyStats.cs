using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName ="EnemyStats", menuName ="ScriptableObject/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public int health;
    public float speedAttack;
    public int damage;

    public float AIUpdateInterval;

    public float acceleration;
    public float angularSpeed;

    public int areaMask;
    public int avoidancePriority;
    public float baseOffset;
    public float height;
    public ObstacleAvoidanceType obstacleAvoidanceType;
    public float radius;
    public float speed;
    public float stoppingDistance;

    public int gemRewardForPlayerWhenKilled;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
