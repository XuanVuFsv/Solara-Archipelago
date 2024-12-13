using UnityEngine;
using UnityEngine.AI;

namespace VitsehLand.Scripts.Stats
{
    [CreateAssetMenu(fileName = "EnemyStat", menuName = "ScriptableObject/EnemyStat")]
    public class EnemyStat : ScriptableObject
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

        public int bulletForce;
        public float shieldDecreaseDamage;
    }
}