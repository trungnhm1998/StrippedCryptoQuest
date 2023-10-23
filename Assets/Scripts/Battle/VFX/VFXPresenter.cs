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

        private void OnEnable() =>
            _castSkillEvent = BattleEventBus.SubscribeEvent<CastSkillEvent>(QueueUpVfxCommand);

        private void OnDisable() => BattleEventBus.UnsubscribeEvent(_castSkillEvent);

        private void OnDestroy()
        {
            _vfxDatabase.ReleaseAllData();
        }

        private void QueueUpVfxCommand(CastSkillEvent ctx)
        {
            var skillVfxId = ctx.Skill.VfxId;
            StartCoroutine(_vfxDatabase.LoadDataById(skillVfxId));
            QueueUpVfx(skillVfxId, ctx.Target);
        }

        public void QueueUpVfx(int vfxId, Components.Character ctxTarget)
        {
            var presentCommand = new VfxCommand(vfxId, ctxTarget.transform.position, this);
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