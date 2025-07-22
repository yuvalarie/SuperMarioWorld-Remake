using System.Collections.Generic;
using Sound;
using Unity.VisualScripting;
using UnityEngine;

namespace Mario.PowerUps.FireMario
{
    /**
     * A pool of objects of type T.
     */
    public class MonoPool<T> : MonoSingleton<MonoPool<T>> where T : MonoBehaviour, IPoolable
    {
        [SerializeField] private int initialPoolSize;
        [SerializeField] private int growthFactor;
        [SerializeField] private T prefab;
        [SerializeField] private Transform parent;
        private Stack<T> _availablePool;

        private new void Awake()
        {
            _availablePool = new Stack<T>();
            AddItemsToPool(initialPoolSize);
        }

        public T Get()
        {
            if (_availablePool.Count == 0)
            {
                AddItemsToPool(growthFactor);
            }

            var obj = _availablePool.Pop();
            obj.gameObject.SetActive(true);
            obj.Reset();
            return obj;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            _availablePool.Push(obj);
        }

        private void AddItemsToPool(int size)
        {
            for (int i = 0; i < size; i++)
            {
                var obj = Instantiate(prefab, parent);
                obj.gameObject.SetActive(false);
                _availablePool.Push(obj);
            }
        }
    }
}