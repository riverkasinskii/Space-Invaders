using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;

namespace ShootEmUp
{
    [RequireComponent(typeof(BulletPool))]
    public sealed class BulletController : MonoBehaviour
    {       
        [SerializeField]
        private LevelBounds levelBounds;

        private BulletPool bulletPool;
        private readonly List<Bullet> m_cache = new();

        private void Awake()
        {
            bulletPool = GetComponent<BulletPool>();
        }

        private void OnEnable() 
            => bulletPool.OnActiveBulletsChanged += OnActiveBulletsChanged;

        private void OnDisable() 
            => bulletPool.OnActiveBulletsChanged -= OnActiveBulletsChanged;

        private void OnActiveBulletsChanged(HashSet<Bullet> activeBullets)
        {
            m_cache.Clear();
            m_cache.AddRange(activeBullets);
        }

        private void FixedUpdate()
            => CheckLevelBoundsForBullet();

        private void CheckLevelBoundsForBullet()
        {       
            for (int i = 0; i < m_cache.Count; i++)
            {
                Bullet bullet = m_cache[i];
                if (!levelBounds.InBounds(bullet.transform.position))
                {
                    bullet.OnCollisionEntered -= OnBulletCollision;
                    bulletPool.RemoveBullet(bullet);
                }
            }
        }                

        public void InitBullet(Vector2 position, Color color, PhysicsLayer physicsLayer, int damage, Vector2 velocity)
        {
            Bullet bullet = bulletPool.SpawnBullet();
            bullet.SetBulletParams(position, color, physicsLayer, damage, velocity);
            bullet.OnCollisionEntered += OnBulletCollision;
        }

        private void OnBulletCollision(Bullet bullet, Collision2D collision)
        {
            DealDamage(bullet, collision.gameObject);
            bulletPool.RemoveBullet(bullet);
        }

        private void DealDamage(Bullet bullet, GameObject other)
        {
            if (other.TryGetComponent(out IEntity entity))
            {
                entity.DealDamage(bullet.Damage);
            }
        }
    }
}
