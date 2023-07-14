using System.Collections;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using CryptoQuest.Gameplay.Battle;

namespace CryptoQuest.UI.Battle
{
    public abstract class CharacterInfoBase : MonoBehaviour
    {
        [SerializeField] protected AttributeScriptableObject _hpAttributeSO;

        protected CharacterDataSO _characterData;
        protected AttributeSystemBehaviour _attributeSystem;

        protected virtual void OnEnable()
        {
            _hpAttributeSO.ValueChangeEvent += OnHPChanged;
        }

        protected virtual void OnDisable()
        {
            _hpAttributeSO.ValueChangeEvent -= OnHPChanged;
        }

        public virtual void SetData(CharacterDataSO data)
        {
            _characterData = data;
            _attributeSystem = data.Owner.AttributeSystem;
            Setup();
        }

        protected abstract void Setup();

        protected abstract void OnHPChanged(AttributeScriptableObject.AttributeEventArgs args);
    }
}