using System;
using CryptoQuest.Audio.SoundEmitters;
using UnityEngine;
using UnityEngine.Pool;

namespace CryptoQuest.Audio
{
    [CreateAssetMenu(menuName = "Crypto Quest/Audio/SoundEmmiter Pool", fileName = "SoundEmitterPoolSO")]
    public class SoundEmitterPoolSO : ScriptableObject
    {
        [SerializeField] private SoundEmitter _prefab = default;

        private IObjectPool<SoundEmitter> _pool;
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

        private void SetParent(Transform t)
        {
            _parent = t;
            PoolRoot.SetParent(_parent);
        }

        public void CreatePool(int poolSize, Transform parent)
        {
            _pool = new ObjectPool<SoundEmitter>(OnCreateSound, OnGetSound, OnReleaseSound, OnDestroySound);

            for (int i = 0; i < poolSize; i++)
            {
                var soundEmitter = _pool.Get();
                SetParent(parent);
                soundEmitter.gameObject.SetActive(false);
            }
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
            soundEmitter.ObjectPool = _pool;
            return soundEmitter;
        }
    }
}