using System;
using UnityEngine;
using Color = UnityEngine.Color;

namespace ShootEmUp
{
    public sealed class Bullet : MonoBehaviour
    {
        public event Action<Bullet, Collision2D> OnCollisionEntered;                        
        public int Damage { get; private set; }

        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEntered?.Invoke(this, collision);
        }

        public void SetBulletParams(Vector2 position, Color color, PhysicsLayer layer, int damage, Vector2 velocity)
        {
            transform.position = position;
            spriteRenderer.color = color;
            gameObject.layer = (int)layer;
            Damage = damage;            
            rb.velocity = velocity;
        }
    }
}