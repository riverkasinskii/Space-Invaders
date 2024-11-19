using UnityEngine;

namespace ShootEmUp
{
    public sealed class LevelBackground : MonoBehaviour
    {
        [SerializeField]
        private float startPositionY;

        [SerializeField]
        private float endPositionY;

        [SerializeField]
        private float movingSpeedY;

        private void FixedUpdate()
        {
            if (transform.position.y <= endPositionY)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    startPositionY,
                    transform.position.z
                );
            }

            transform.position -= new Vector3(
                transform.position.x,
                movingSpeedY * Time.fixedDeltaTime,
                transform.position.z
            );
        }
    }
}