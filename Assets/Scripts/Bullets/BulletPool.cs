using System;
using System.Collections.Generic;

namespace ShootEmUp
{
    public sealed class BulletPool : Pool<Bullet>
    {
        public event Action<HashSet<Bullet>> OnActiveBulletsChanged;                        

        public Bullet SpawnBullet()
        {
            Bullet bullet = TryGetInstance();           
            AddActiveElements(bullet);
            OnActiveBulletsChanged.Invoke(activePool);
            return bullet;
        }

        public void RemoveBullet(Bullet bullet)
        {
            OnActiveBulletsChanged.Invoke(activePool);
            RemoveActiveElements(bullet);
        }                                    
    }
}