using CryptoQuest.Gameplay.BaseGameplayData;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character
{
    /// <summary>
    /// Structure for general character, this will only have persist data of character
    /// <para>Example: <see cref="EnemyData"/></para> 
    /// </summary>
    [CreateAssetMenu(fileName = "NewCharacter", menuName = "Gameplay/Character/Character")]
    public class CharacterData<TData, TInformation> : GenericData
        where TData : CharacterData<TData, TInformation>
        where TInformation : CharacterInformation<TData, TInformation>, new()
    {
        [field: SerializeField] public Elemental Element { get; private set; }

        /// <summary>
        /// Factory method
        /// Need to create character info when there're
        /// many characters using the same data set, like enemies
        /// </summary>
        /// <returns></returns>
        public TInformation CreateCharacterSpec()
        {
            Debug.Log($"Create character from {name}" +
                      $"\nis {typeof(TData)} equals {GetType()} : {this is TData}");
            var character = new TInformation();
            character.Init((TData)this);
            return character;
        }
    }
}