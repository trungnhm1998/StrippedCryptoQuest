using System;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Battle.CharacterInfo
{
    [Obsolete]
    public abstract class CharacterInfoBase : MonoBehaviour
    {
        [SerializeField] protected AttributeScriptableObject _hpAttributeSO;

        protected CharacterInformation _characterInfo;
        protected AttributeSystemBehaviour _attributeSystem;

        protected virtual void OnDisable()
        {
            _attributeSystem.PostAttributeChange -= OnHPChanged;
        }

        public virtual void SetData(CharacterInformation characterInfo)
        {
            _characterInfo = characterInfo;
            _attributeSystem = characterInfo.Owner.AttributeSystem;
            _attributeSystem.PostAttributeChange += OnHPChanged;
            Setup();
        }

        public virtual void ShowSelected(string name)
        {
            OnSelected(name);
        }

        public abstract void SetSelectActive(bool value);

        protected abstract void Setup();

        protected abstract void OnHPChanged(AttributeScriptableObject attribute, AttributeValue oldValue,
            AttributeValue newValue);

        protected abstract void OnSelected(string name);
    }
}