using System;
using System.Collections;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.Presenter;
using CryptoQuest.Battle.Presenter.Commands;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.VFX
{
    public class VFXPresenter : MonoBehaviour
    {
        [SerializeField] private RoundEventsPresenter _roundEventsPresenter;
        [SerializeField] private VFXDatabase _vfxDatabase;
        private TinyMessageSubscriptionToken _castSkillEvent;
        private TinyMessageSubscriptionToken _playVfxEvent;

        private void OnEnable()
        {
            _castSkillEvent = BattleEventBus.SubscribeEvent<CastSkillEvent>(QueueUpVfxCommand);
            _playVfxEvent = BattleEventBus.SubscribeEvent<PlayVfxEvent>(QueueVfxCommand);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_castSkillEvent);
            BattleEventBus.UnsubscribeEvent(_playVfxEvent);
        }

        private void OnDestroy()
        {
            _vfxDatabase.ReleaseAllData();
        }

        private void QueueUpVfxCommand(CastSkillEvent ctx)
        {
            var skillVfxId = ctx.Skill.VfxId;
            QueueVfxCommand(ctx, skillVfxId);
        }
        
        private void QueueVfxCommand(PlayVfxEvent ctx)
        {
            StartCoroutine(_vfxDatabase.LoadDataById(ctx.VfxId));
            QueueUpVfx(ctx.VfxId, Vector3.zero);
        }

        private void QueueVfxCommand(CastSkillEvent ctx, int skillVfxId)
        {
            StartCoroutine(_vfxDatabase.LoadDataById(skillVfxId));
            QueueUpVfx(skillVfxId, ctx.Target.transform.position);
        }

        public void QueueUpVfx(int vfxId, Vector3 position = default)
        {
            var presentCommand = new VfxCommand(vfxId, position, this);
            _roundEventsPresenter.EnqueueCommand(presentCommand);
        }

        public IEnumerator PresentVfx(int vfxId, Vector3 transformPosition)
        {
            yield return _vfxDatabase.LoadDataById(vfxId); // just to make sure
            var vfxPrefab = _vfxDatabase.GetDataById(vfxId);
            ParticleSystemRenderer[] childs = vfxPrefab.GetComponentsInChildren<ParticleSystemRenderer>();
            foreach (ParticleSystemRenderer psr in childs)
            {
                psr.sortingOrder = 1000;
                psr.sortingLayerName = "Battle";
            }

            var vfx = Instantiate(vfxPrefab, transformPosition, Quaternion.identity);
            yield return new WaitUntil(() => vfx == null);
        }
    }
}