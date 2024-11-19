using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public class Pool<T> : MonoBehaviour where T : Component
    {
        [SerializeField]
        private int startSizePool = 7;

        [SerializeField]
        protected Transform activeContainer;

        [SerializeField]
        protected Transform startContainer;

        [SerializeField]
        protected T prefab;
                
        protected readonly Queue<T> startPool = new();
        protected readonly HashSet<T> activePool = new();

        private void Awake() => InitStartPool();

        private void InitStartPool()
        {
            for (var i = 0; i < startSizePool; i++)
            {
                T go = Instantiate(prefab, startContainer);
                startPool.Enqueue(go);
            }
        }

        protected T TryGetInstance()
        {
            if (!startPool.TryDequeue(out T instance))
            {
                instance = Instantiate(prefab, startContainer);
            }
            return instance;
        }

        protected void AddActiveElements(T element)
        {
            activePool.Add(element);
            element.transform.SetParent(activeContainer);
        }

        protected void RemoveActiveElements(T element)
        {
            element.transform.SetParent(startContainer);
            activePool.Remove(element);
            startPool.Enqueue(element);
        }
    }
}
