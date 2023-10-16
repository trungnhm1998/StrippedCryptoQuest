using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Ability;
using CryptoQuest.Battle.Events;
using TinyMessenger;

namespace CryptoQuest.Battle.VFX
{
    public class VFXPresenter : MonoBehaviour
    {
        [SerializeField] private VFXDatabase _vfxDatabase;
        private TinyMessageSubscriptionToken _castSkillEvent;

        private void OnEnable()
        {
            _castSkillEvent = BattleEventBus.SubscribeEvent<CastSkillEffectEvent>(CastSkillVFX);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_castSkillEvent);
        }

        private void CastSkillVFX(CastSkillEffectEvent ctx)
        {
            StartCoroutine(OnPreAttack(ctx));
        }    

        public IEnumerator OnPreAttack(CastSkillEffectEvent ctx)
        {
            if(ctx.Target is not EnemyBehaviour)
            {
                Debug.Log("Not target enemy");
                yield break;
            }
            string vfxId = ctx.Skill.Parameters.SkillParameters.VfxId;
            yield return _vfxDatabase.LoadDataById(vfxId);
            var vfx = _vfxDatabase.GetDataById(vfxId);
            if(vfx != null)
            {
                vfx.Init();
                yield return vfx.Execute(ctx.Target.transform);
            }
            ctx.OnComplete?.Invoke();
        }
    }
}
