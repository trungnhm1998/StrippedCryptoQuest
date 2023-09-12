using CryptoQuest.Gameplay.BaseGameplayData;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character
{
    /// <summary>
    /// Structure for general character, this will only have persist data of character
    /// <para>Example: <see cref="EnemyDef"/></para> 
    /// </summary>
    public abstract class CharacterData<TDef, TSpec> : GenericData
        where TDef : CharacterData<TDef, TSpec>
        where TSpec : CharacterInformation<TDef, TSpec>, new()
    {
        [field: SerializeField] public Elemental Element { get; private set; }

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
#if UNITY_EDITOR
        public void Editor_SetId(int id)
        {
            Id = id;
        }

        public void Editor_SetName(LocalizedString name)
        {
            Name = name;
        }

        public void Editor_SetDescription(LocalizedString description)
        {
            Description = description;
        }

        public void Editor_SetElement(Elemental element)
        {
            Element = element;
        }
#endif
    }
}