using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.VFX;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Presenter.Commands
{
    public class NormalAttackPresenter : MonoBehaviour
    {
        [SerializeField] private VFXPresenter _vfxPresenter;
        [SerializeField] private int _vfxId = 0;
        private TinyMessageSubscriptionToken _normalAttackEvent;

        private void Awake() => _normalAttackEvent =
            BattleEventBus.SubscribeEvent<HeroNormalAttackEvent>(EnqueueNormalAttackVfx);

        private void OnDestroy() => BattleEventBus.UnsubscribeEvent(_normalAttackEvent);

        private void EnqueueNormalAttackVfx(HeroNormalAttackEvent ctx) =>
            _vfxPresenter.QueueUpVfx(_vfxId, ctx.Target.transform.position);
    }
}