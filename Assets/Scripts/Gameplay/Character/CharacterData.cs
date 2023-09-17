using CryptoQuest.Character.Enemy;
using CryptoQuest.Gameplay.BaseGameplayData;
using CryptoQuest.Gameplay.Enemy;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character
{
    /// <summary>
    /// Structure for general character, this will only have persist data of character
    /// <para>Example: <see cref="EnemyDef"/></para> 
    /// </summary>
    public abstract class CharacterData<TDef, TSpec> : ScriptableObject
        where TDef : CharacterData<TDef, TSpec>
        where TSpec : CharacterInformation<TDef, TSpec>, new()
    {
        /// <summary>
        /// Factory method
        /// Need to create character info when there're
        /// many characters using the same data set, like enemies
        /// </summary>
        /// <returns></returns>
        public TSpec CreateCharacterSpec()
        {
            Debug.Log($"Create character from {name}" +
                      $"\nis {typeof(TDef)} equals {GetType()} : {this is TDef}");
            var character = new TSpec();
            character.Init((TDef)this);
            return character;
        }
    }
}