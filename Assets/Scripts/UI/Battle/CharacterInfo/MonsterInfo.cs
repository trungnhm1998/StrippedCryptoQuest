using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Battle.CharacterInfo
{
    public class MonsterInfo : CharacterInfoBase
    {
        // TODO: Replace with spine animation
        [SerializeField]
        private SpriteRenderer monsterSprite;

        protected override void Setup()
        {
            monsterSprite.sprite = _characterInfo.Data.BattleIconSprite;
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