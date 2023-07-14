using UnityEngine;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem;

namespace CryptoQuest.UI.Battle
{
    public class MonsterInfo : CharacterInfoBase
    {
        // TODO: Replace with spine animation
        [SerializeField]
        private SpriteRenderer monsterSprite;

        protected override void Setup()
        {
            monsterSprite.sprite = _characterData.BattleIconSprite;
        }

        protected override void OnHPChanged(AttributeScriptableObject.AttributeEventArgs args)
        {
            if (args.System != _characterData.Owner.AttributeSystem) return;

            _characterData.Owner.AttributeSystem.GetAttributeValue(_hpAttributeSO, out AttributeValue hpValue);
            if (hpValue.CurrentValue <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}