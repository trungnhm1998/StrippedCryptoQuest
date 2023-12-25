using System;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.Menus.Home.UI.CharacterList
{
    public class CharacterListPresenter : MonoBehaviour
    {
        [SerializeField] private UICharacterList _uiCharacterList;
        [SerializeField] private UICharacterDetails _characterDetails;
        [SerializeField] private HeroBehaviour _heroBehaviour;

        private void OnEnable()
        {
            _uiCharacterList.InspectingHero += UpdateDetails;
        }

        private void OnDisable()
        {
            _uiCharacterList.InspectingHero -= UpdateDetails;
        }

        private void UpdateDetails(HeroSpec spec)
        {
            _heroBehaviour.Spec = spec;
            _heroBehaviour.GetComponent<Element>().SetElement(spec.Elemental);
            _heroBehaviour.Init();
            
            _characterDetails.InspectCharacter(_heroBehaviour);
        }
    }
}