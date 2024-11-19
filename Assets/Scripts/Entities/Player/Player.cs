using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Player : MonoBehaviour, IEntity
    {
        public event Action<int> OnHealthChanged;
        public event Action OnHealthEnded;

        public Transform FirePoint { get => firePoint; }
        public int Damage { get => damage; }

        [SerializeField]
        private Transform firePoint;
        
        [SerializeField]
        private int health;

        [SerializeField]
        private int damage = 1;

        [SerializeField]
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private float speed = 5.0f;                

        public void Move(int direction)
        {
            Vector2 moveDirection = new(direction, 0);
            Vector2 moveStep = speed * Time.fixedDeltaTime * moveDirection;
            Vector2 targetPosition = _rigidbody.position + moveStep;
            _rigidbody.MovePosition(targetPosition);
        }

        public void DealDamage(int damage)
        {            
            health = Mathf.Max(0, health - damage);
            OnHealthChanged?.Invoke(health);

            if (health <= 0)
            {
                OnHealthEnded?.Invoke();
            }                
        }
    }
}