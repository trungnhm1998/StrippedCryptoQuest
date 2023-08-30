using System;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.UI.Menu.Panels.Home;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character
{
    /// <summary>
    /// Use this to save game
    /// </summary>
    [Serializable]
    public class CharacterSpec
    {
        [field: SerializeField] public CharacterBase BaseInfo { get; set; }
        [field: SerializeField] public Elemental Element { get; set; }
        [field: SerializeField] public int Level { get; set; }
        [field: SerializeField] public StatsDef StatsDef { get; set; }
        [field: SerializeField] public CharacterEquipments Equipments { get; private set; }
        public Sprite Avatar => BaseInfo.Avatar;

        private CharacterBehaviourBase _characterComponent;
        public CharacterBehaviourBase CharacterComponent => _characterComponent;

        public bool IsValid()
        {
            return BaseInfo != null
                   && Element != null
                   && StatsDef.Attributes.Length > 0;
        }

        public void Bind(CharacterBehaviourBase characterBehaviour)
        {
            _characterComponent = characterBehaviour;
            Equipments.ClearEventRegistration();
        }

        public void SetupUI(ICharacterInfo uiCharacterInfo)
        {
            uiCharacterInfo.SetElement(Element.Icon);
            uiCharacterInfo.SetLevel(Level);
            BaseInfo.SetupUI(uiCharacterInfo);
        }
    }
}