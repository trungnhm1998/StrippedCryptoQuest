using System.Collections;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.Character;
using CryptoQuest.Inventory;
using UnityEngine;

namespace CryptoQuest.ChangeClass
{
    public class ClassBerserkerController : MonoBehaviour
    {
        [field: SerializeField] public UIClassMaterial BerserkerClassMaterials { get; private set; }
        [SerializeField] private UIItemMaterial _uiMaterials;
        [SerializeField] private CharacterClass _classBerserker;
        [SerializeField] private GameObject _previewClassMaterial;
        [SerializeField] private GameObject _berserkerMateiralPanel;
        private UIOccupation _occupation;
        public bool IsValid { get; private set; }

        public void HandleSelectedOccupation(HeroInventorySO heroInventory, UIOccupation occupation)
        {
            _occupation = occupation;
            IsValid = false;
            if (_occupation.Class.CharacterClass != _classBerserker)
            {
                _berserkerMateiralPanel.gameObject.SetActive(false);
                _previewClassMaterial.gameObject.SetActive(true);
            }
            else
            {
                _berserkerMateiralPanel.gameObject.SetActive(true);
                _previewClassMaterial.gameObject.SetActive(false);
                StartCoroutine(BerserkerClassMaterials.InstantiateDataForBerserker(heroInventory.OwnedHeroes, _occupation));
                StartCoroutine(ValidateChangeClassMaterial());
            }
        }

        private IEnumerator ValidateChangeClassMaterial()
        {
            yield return new WaitUntil(() => BerserkerClassMaterials.IsFinishInstantiateData);
            IsValid = IsValidClassMateiral();
            _occupation.EnableDefaultBackground(IsValid);
        }

        private bool IsValidClassMateiral()
        {
            return BerserkerClassMaterials.ListClassCharacter.Count > 0 && _uiMaterials.IsValid &&
                   !BerserkerClassMaterials.IsEmptyMaterial;
        }
    }
}