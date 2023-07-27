using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Battle.CharacterInfo
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

        public virtual void ShowSelected(string name)
        {
            OnSelected(name);
        }

        protected abstract void Setup();

        protected abstract void OnHPChanged(AttributeScriptableObject.AttributeEventArgs args);

        protected abstract void OnSelected(string name);
    }
}