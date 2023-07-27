using UnityEngine;
using UnityEngine.Pool;

namespace CryptoQuest.Audio.SoundEmitters
{
    public class SoundEmitterPool : MonoBehaviour
    {
        [SerializeField] private SoundEmitter _prefab = default;

        private IObjectPool<SoundEmitter> _pool;
        private Transform _poolRoot;

        private const bool COLLECTION_CHECK = true;
        private const int DEFAULT_POOL_SIZE = 10;

        private Transform PoolRoot
        {
            get
            {
                if (_poolRoot == null)
                {
                    _poolRoot = new GameObject(name).transform;
                    _poolRoot.SetParent(_parent);
                }

                return _poolRoot;
            }
        }

        private Transform _parent;

        public void SetParent(Transform t)
        {
            _parent = t;
            PoolRoot.SetParent(_parent);
        }

        public void Create(int poolSize)
        {
            _pool = new ObjectPool<SoundEmitter>(OnCreateSound, OnGetSound, OnReleaseSound, OnDestroySound,
                COLLECTION_CHECK, DEFAULT_POOL_SIZE, poolSize);
        }

        public SoundEmitter Request()
        {
            return _pool.Get();
        }

        public void Release(SoundEmitter soundEmitter)
        {
            soundEmitter.ReleasePool();
        }

        private void OnDestroySound(SoundEmitter obj)
        {
            Destroy(obj.gameObject);
        }

        private void OnReleaseSound(SoundEmitter obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnGetSound(SoundEmitter obj)
        {
            obj.transform.SetAsFirstSibling();
            obj.gameObject.SetActive(true);
        }

        private SoundEmitter OnCreateSound()
        {
            var soundEmitter = Instantiate(_prefab, PoolRoot.transform);
            soundEmitter.Init(_pool);
            return soundEmitter;
        }
    }
}