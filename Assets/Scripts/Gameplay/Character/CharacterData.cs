using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Character
{
    /// <summary>
    /// structure for [monster](https://docs.google.com/spreadsheets/d/1WkX1DyDOGf6EiAppo8Buz2sUkSKV5OnDENEvmHzKXNQ/edit#gid=1024080951)
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Gameplay/Character/Character Data")]
    public class CharacterData : ScriptableObject
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public LocalizedString Name { get; private set; }
        [field: SerializeField] public LocalizedString Description { get; private set; }
        [field: SerializeField] public Elemental Element { get; private set; }

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