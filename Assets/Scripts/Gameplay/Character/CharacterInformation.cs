using System;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;

namespace CryptoQuest.Gameplay.Character
{
    [Serializable]
    public class CharacterInformation
    {
        private CharacterData _data;

        public CharacterInformation(CharacterData characterData)
        {
            _data = characterData;
        }
    }
}