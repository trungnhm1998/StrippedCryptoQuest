using System;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character
{
    public interface ICharacter
    {
        CharacterData Data { get; }
        CharacterBehaviourBase CharacterComponent { get; }
        Elemental Element { get; }

        void Init(CharacterBehaviourBase characterBehaviour);
        bool IsValid();
    }

    [Serializable]
    public class CharacterInformation : ICharacter
    {
        private CharacterData _data;
        public CharacterData Data => _data;

        private CharacterBehaviourBase _characterComponent;
        public CharacterBehaviourBase CharacterComponent => _characterComponent;

        [field: SerializeField] public Elemental Element { get; set; }

        public CharacterInformation(CharacterData characterData)
        {
            _data = characterData;
        }

        public void Init(CharacterBehaviourBase characterBehaviour)
        {
            _characterComponent = characterBehaviour;
        }

        public bool IsValid()
        {
            return _data != null;
        }
    }
}