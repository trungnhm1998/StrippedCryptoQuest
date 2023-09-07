using System;
using UnityEngine;
using UnityEngine.Localization;
using CryptoQuest.Gameplay.BaseGameplayData;
using UnityEngine.Analytics;

namespace CryptoQuest.Gameplay.Character
{
    /// <summary>
    /// Structure for general character, this will only have persist data of character
    /// <para>Example: <see cref="EnemyData"/></para> 
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Gameplay/Character/Character Data")]
    public class CharacterData : GenericData
    {
        /// <summary>
        /// Factory method
        /// Need to create character info when there're
        /// many characters using the same data set, like enemies
        /// </summary>
        /// <returns></returns>
        public CharacterInformation CreateCharacterInfo()
        {
            return new CharacterInformation(this);
        }
    }
}