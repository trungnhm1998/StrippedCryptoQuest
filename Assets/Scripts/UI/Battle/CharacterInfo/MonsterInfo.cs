using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;
using System.Collections;
using Spine.Unity;

namespace CryptoQuest.UI.Battle.CharacterInfo
{
    public class MonsterInfo : CharacterInfoBase
    {
        // To hide spine object until it loaded and reset position
        private const float INIT_Z_OFFSET = 100f;

        public SkeletonAnimation SpineAnimation { get; set; }

        protected override void Setup()
        {
            //TODO: Refactor this later
            if (_characterInfo.Data is MonsterDataSO monsterData)
            {
                StartCoroutine(LoadMonsterPrefab(monsterData));
            }
        }

        private IEnumerator LoadMonsterPrefab(MonsterDataSO data)
        {
            var handle = data.MonsterPrefab.InstantiateAsync(Vector3.zero + Vector3.back * INIT_Z_OFFSET, Quaternion.identity, transform);
            yield return handle;
            if (!handle.IsDone) yield break;
            LoadPrefabComplete(handle.Result);
        }

        private void LoadPrefabComplete(GameObject monsterGO)
        {
            monsterGO.transform.localPosition = Vector3.zero;
        }

        protected override void OnHPChanged(AttributeScriptableObject.AttributeEventArgs args)
        {
            if (args.System != _characterInfo.Owner.AttributeSystem) return;

            _characterInfo.Owner.AttributeSystem.GetAttributeValue(_hpAttributeSO, out AttributeValue hpValue);
            if (hpValue.CurrentValue <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        protected override void OnSelected(string name) { }
    }
}