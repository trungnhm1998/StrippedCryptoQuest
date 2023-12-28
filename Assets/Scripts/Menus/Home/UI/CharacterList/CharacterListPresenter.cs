using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero;
using CryptoQuest.Inventory;
using UnityEngine;

namespace CryptoQuest.Menus.Home.UI.CharacterList
{
    public class CharacterListPresenter : MonoBehaviour
    {
        [SerializeField] private UICharacterList _uiCharacterList;
        [SerializeField] private UICharacterDetails _characterDetails;
        [SerializeField] private HeroBehaviour _heroBehaviour;
        [SerializeField] private HeroInventorySO _inventory;

        private void OnEnable()
        {
            _uiCharacterList.InspectingHero += UpdateDetails;
            Init();
        }

        private void OnDisable()
        {
            _uiCharacterList.InspectingHero -= UpdateDetails;
            _uiCharacterList.gameObject.SetActive(true);
            _characterDetails.gameObject.SetActive(true);
        }

        private void UpdateDetails(HeroSpec spec)
        {
            _heroBehaviour.Spec = spec;
            _heroBehaviour.GetComponent<Element>().SetElement(spec.Elemental);
            _heroBehaviour.Init();

            _characterDetails.InspectCharacter(_heroBehaviour);
        }

        private void Init()
        {
            if (_inventory.OwnedHeroes.Count > 0) return;
            _uiCharacterList.gameObject.SetActive(false);
            _characterDetails.gameObject.SetActive(false);
        }
    }
}