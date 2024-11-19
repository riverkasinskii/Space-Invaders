using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShootEmUp
{
    public sealed class EnemyController : MonoBehaviour
    {
        public event Action<Enemy> OnEnemyKilled;

        [SerializeField]
        private Transform[] spawnPositions;

        [SerializeField]
        private Transform[] attackPositions;
        
        [SerializeField]
        private Player target;

        [SerializeField]
        private BulletController bulletController;

        [SerializeField]
        private float countdown;
                
        public void InitActiveEnemies(Enemy enemy)
        {      
            SetSpawnPosition(enemy);
            SetAttackPosition(enemy);
            Subscribe(enemy);
        }                
                
        private void OnEnemyHealthEnded(Enemy enemy)
        {
            OnEnemyKilled?.Invoke(enemy);
            UnSubscribe(enemy);
        }                

        private void Subscribe(Enemy enemy)
        {
            enemy.OnFire += OnFire;
            enemy.OnEnemyHealthEnded += OnEnemyHealthEnded;
        }

        private void UnSubscribe(Enemy enemy)
        {
            enemy.OnFire -= OnFire;
            enemy.OnEnemyHealthEnded -= OnEnemyHealthEnded;
        }

        private void SetAttackPosition(Enemy enemy)
        {
            Transform attackPosition = RandomPoint(attackPositions);
            StartCoroutine(StartEnemyMove(enemy, attackPosition.position));            
        }

        private void SetSpawnPosition(Enemy enemy)
        {
            Transform spawnPosition = RandomPoint(spawnPositions);
            enemy.transform.position = spawnPosition.position;
        }

        private IEnumerator StartEnemyMove(Enemy enemy, Vector2 position)
        {
            while (true)
            {
                enemy.Move(position);
                if (enemy.IsPointReached)
                {
                    StartCoroutine(StartEnemyShoot(enemy));
                    yield break;
                }
                yield return null;
            }
        }

        private IEnumerator StartEnemyShoot(Enemy enemy)
        {
            while (true)
            {
                if (enemy.IsEnemyKilled)
                {
                    yield break;
                }
                enemy.Shoot(target.transform.position);                
                yield return new WaitForSeconds(countdown);
            }
        }                                          

        private void OnFire(Vector2 position, Vector2 direction, int damage)
        {
            bulletController.InitBullet(
                position,
                Color.red,
                PhysicsLayer.ENEMY_BULLET,
                damage,                
                direction * 2
            );
        }

        private Transform RandomPoint(Transform[] points)
        {
            int index = Random.Range(0, points.Length);
            return points[index];
        }
    }
}