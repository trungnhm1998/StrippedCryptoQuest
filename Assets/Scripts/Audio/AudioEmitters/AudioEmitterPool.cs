using UnityEngine;
using UnityEngine.Pool;

namespace CryptoQuest.Audio.AudioEmitters
{
    public class AudioEmitterPool : MonoBehaviour
    {
        private const bool COLLECTION_CHECK = true;
        private const int DEFAULT_POOL_SIZE = 10;

        [SerializeField] private AudioEmitter _prefab = default;

        private IObjectPool<AudioEmitter> _pool;
        private Transform _poolRoot;

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
            _pool = new ObjectPool<AudioEmitter>(OnCreateSound, OnGetSound, OnReleaseSound, OnDestroySound,
                COLLECTION_CHECK, DEFAULT_POOL_SIZE, poolSize);
        }

        public AudioEmitter Request()
        {
            return _pool.Get();
        }

        public void Release(AudioEmitter audioEmitter)
        {
            audioEmitter.ReleasePool();
        }

        private void OnDestroySound(AudioEmitter obj)
        {
            Destroy(obj.gameObject);
        }

        private void OnReleaseSound(AudioEmitter obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnGetSound(AudioEmitter obj)
        {
            obj.transform.SetAsFirstSibling();
            obj.gameObject.SetActive(true);
        }

        private AudioEmitter OnCreateSound()
        {
            var soundEmitter = Instantiate(_prefab, PoolRoot.transform);
            soundEmitter.Init(_pool);
            return soundEmitter;
        }
    }
}