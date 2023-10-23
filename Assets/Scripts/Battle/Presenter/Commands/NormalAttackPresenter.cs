using System.Collections;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Battle.Presenter.Commands
{
    public class NormalAttackPresenter : MonoBehaviour
    {
        [SerializeField] private RoundEventsPresenter _roundEventsPresenter;
        [SerializeField] private AssetReference _visualEffect;
        private TinyMessageSubscriptionToken _normalAttackEvent;
        private GameObject _vfx;
        private AsyncOperationHandle<GameObject> _handle;

        private void Awake()
        {
            _normalAttackEvent = BattleEventBus.SubscribeEvent<HeroNormalAttackEvent>(EnqueueNormalAttackVfx);
        }

        private IEnumerator Start()
        {
            _handle = _visualEffect.LoadAssetAsync<GameObject>();
            yield return _handle;
            if (_handle.Status != AsyncOperationStatus.Succeeded) yield break;
            _vfx = _handle.Result;
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_normalAttackEvent);
            Addressables.Release(_handle);
        }

        private void EnqueueNormalAttackVfx(HeroNormalAttackEvent ctx)
        {
            var presentCommand = new VfxCommand(_vfx, ctx.Target.transform.position, this);
            _roundEventsPresenter.EnqueueCommand(presentCommand);
        }

        public GameObject InstantiateVfx(GameObject prefab, Vector3 position, Quaternion rotation) =>
            Instantiate(prefab, position, rotation);
    }

    internal class VfxCommand : IPresentCommand
    {
        private AssetReference _visualEffect;
        private readonly Vector3 _transformPosition;
        private readonly GameObject _vfx;
        private readonly NormalAttackPresenter _presenter;

        public VfxCommand(GameObject visualEffect, Vector3 transformPosition, NormalAttackPresenter presenter)
        {
            _presenter = presenter;
            _transformPosition = transformPosition;
            _vfx = visualEffect;
        }

        public IEnumerator Present()
        {
            var vfx = _presenter.InstantiateVfx(_vfx, _transformPosition, Quaternion.identity);
            yield return new WaitUntil(() => vfx == null);
        }
    }
}