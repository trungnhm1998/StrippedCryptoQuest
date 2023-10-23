using System.Collections;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace CryptoQuest.Battle.Presenter.Commands
{
    public class NormalAttackVfx : MonoBehaviour
    {
        [FormerlySerializedAs("_vfxAndLogPresenter")] [SerializeField] private RoundEventsPresenter _roundEventsPresenter;
        [SerializeField] private AssetReference _visualEffect;
        private TinyMessageSubscriptionToken _normalAttackEvent;

        private void Awake()
        {
            _normalAttackEvent = BattleEventBus.SubscribeEvent<HeroNormalAttackEvent>(EnqueueNormalAttackVfx);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_normalAttackEvent);
        }
        
        private void EnqueueNormalAttackVfx(HeroNormalAttackEvent ctx)
        {
            _roundEventsPresenter.EnqueueCommand(new VfxCommand(_visualEffect, ctx.Target.transform.position));
        }
    }

    internal class VfxCommand : IPresentCommand
    {
        private AssetReference _visualEffect;
        private Vector3 _transformPosition;

        public VfxCommand(AssetReference visualEffect, Vector3 transformPosition)
        {
            _transformPosition = transformPosition;
            _visualEffect = visualEffect;
        }

        public IEnumerator Present()
        {
            yield break;
        }
    }
}