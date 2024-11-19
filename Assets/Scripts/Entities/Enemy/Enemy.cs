using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Enemy : MonoBehaviour, IEntity
    {
        public event Action<Enemy> OnEnemyHealthEnded;
                
        public event Action<Vector2, Vector2, int> OnFire;

        public event Action<bool> OnDestinationCompleted;

        [SerializeField]
        private Transform firePoint;
        
        [SerializeField]
        private int health = 3;

        private int startHealth;

        [SerializeField]
        private int damage = 1;

        [SerializeField]
        private Rigidbody2D rb;

        [SerializeField]
        private float speed = 5.0f;

        public bool IsPointReached { get; private set; } = false;
        public bool IsEnemyKilled { get; private set; } = false;

        private void Start()
        {
            startHealth = health;
        }

        public void DealDamage(int damage)
        {
            if (health > 0)
            {
                health = Mathf.Max(0, health - damage);
            }
            if (health <= 0)
            {
                IsEnemyKilled = true;
                OnEnemyHealthEnded?.Invoke(this);
                ResetStates();                
            }
        }              

        public void Move(Vector2 destination)
        {
            Vector2 vector = destination - (Vector2)transform.position;
            if (vector.magnitude <= 0.25f)
            {
                IsPointReached = true;
                OnDestinationCompleted?.Invoke(IsPointReached);
                return;
            }

            Vector2 direction = vector.normalized * Time.fixedDeltaTime;
            Vector2 nextPosition = rb.position + direction * speed;
            rb.MovePosition(nextPosition);
        }

        public void Shoot(Vector2 targetPosition)
        {
            Vector2 startPosition = firePoint.position;
            Vector2 vector = targetPosition - startPosition;
            Vector2 direction = vector.normalized;
            OnFire?.Invoke(startPosition, direction, damage);
        }

        private void ResetStates()
        {
            IsPointReached = false;
            IsEnemyKilled = false;
            health = startHealth;
        }
    }
}