using CryptoQuest.Character.Beast;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIBeastDetail : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _beastName;
        [SerializeField] private LocalizeStringEvent _beastPassiveSkill;
        [SerializeField] private Image _beastImage;
        [SerializeField] private Image _beastElement;

        private void OnEnable()
        {
            UIBeast.Inspecting += OnInspectingBeast;
        }

        private void OnDisable()
        {
            UIBeast.Inspecting -= OnInspectingBeast;
        }

        private void OnInspectingBeast(BeastDef beastDef)
        {
            _beastName.StringReference = beastDef.Data.BeastTypeSo.BeastInformation.LocalizedName;

            _beastPassiveSkill.StringReference =
                beastDef.Data.Passives != null ? beastDef.Data.Passives.Description : new LocalizedString();

            _beastElement.sprite = beastDef.Data.Elemental.Icon;
        }
    }
}