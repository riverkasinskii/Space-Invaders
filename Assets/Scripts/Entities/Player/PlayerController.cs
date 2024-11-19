using UnityEngine;

namespace ShootEmUp
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Player player;

        [SerializeField]
        private BulletController bulletController;

        private void OnEnable() 
            => player.OnHealthEnded += HealthEmptyListener;

        private void OnDisable() 
            => player.OnHealthEnded -= HealthEmptyListener;

        private void HealthEmptyListener() 
            => Time.timeScale = 0;

        private void Update()
        {
            ShootListener();
            MoveListener();
        }

        private void MoveListener()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                player.Move(-1);
            }
            else if(Input.GetKey(KeyCode.RightArrow))
            {
                player.Move(1);
            }
            else
            {
                player.Move(0);
            }
        }

        private void ShootListener()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bulletController.InitBullet(
                    player.FirePoint.position,
                    Color.blue,
                    PhysicsLayer.PLAYER_BULLET,
                    player.Damage,
                    player.FirePoint.rotation * Vector3.up * 3
                );
            }
        }
    }
}