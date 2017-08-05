using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public delegate IPoolable PoolObjectAllocator();

    public interface IPoolable
    {
        void OnReturnToPool(Transform poolParent);
        void OnGetFromPool();
    }

    public class ObjectPool<T> where T : IPoolable
    {
        private readonly Queue<T> _availableObjects;
        private readonly HashSet<T> _inUse;
        private readonly int _allocationSize;
        private readonly Transform _poolParent;
        private readonly PoolObjectAllocator _allocator;

        public ObjectPool(int initialSize, PoolObjectAllocator allocationMethod, int allocationSize = 32)
        {
            _availableObjects = new Queue<T>(initialSize);
            _inUse = new HashSet<T>();
            _allocator = allocationMethod;
            _allocationSize = allocationSize;

            _poolParent = new GameObject(typeof(T).Name + "Pool").GetComponent<Transform>();

            for (int i = 0; i < initialSize; i++)
            {
                var obj = allocationMethod();
                obj.OnReturnToPool(_poolParent);
                _availableObjects.Enqueue((T)obj);
            }
        }

        public T GetObject()
        {
            if (_availableObjects.Count == 0)
            {
                ExtendPool();
            }

            var obj = _availableObjects.Dequeue();
            _inUse.Add(obj);
            obj.OnGetFromPool();

            return obj;
        }

        public void ReleaseObject(T obj)
        {
            if (_inUse.Contains(obj))
            {
                obj.OnReturnToPool(_poolParent);
                _availableObjects.Enqueue(obj);
            }
        }

        private void ExtendPool()
        {
            for (int i = 0; i < _allocationSize; i++)
            {
                var obj = _allocator();
                obj.OnReturnToPool(_poolParent);
                _availableObjects.Enqueue((T)obj);
            }
        }
    }
}