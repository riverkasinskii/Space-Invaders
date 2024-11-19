using System.Collections;
using UnityEngine;

namespace ShootEmUp
{
    [RequireComponent(typeof(EnemyController))]
    public sealed class EnemyPool : Pool<Enemy>
    {                      
        private EnemyController enemyController;        

        private void Awake() 
            => enemyController = GetComponent<EnemyController>();

        private void OnEnable()
            => enemyController.OnEnemyKilled += RemoveActiveElements;

        private void OnDisable()
            => enemyController.OnEnemyKilled -= RemoveActiveElements;

        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(1, 2));                                

                if (activePool.Count < 5)
                {
                    Enemy enemy = TryGetInstance();
                    AddActiveElements(enemy);
                    enemyController.InitActiveEnemies(enemy);
                }
            }
        }                            
    }
}
