using CryptoQuest.Character.Beast;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIBeastDetail : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _beastName;
        [SerializeField] private LocalizeStringEvent _localizedPassiveSkill;
        [SerializeField] private TMP_Text _txtPassiveSkill;
        [SerializeField] private Image _beastImage;
        [SerializeField] private Image _beastElement;

        public void FillUI(IBeast beast)
        {
            _txtPassiveSkill.text = "";
            _beastName.StringReference = beast.LocalizedName;

            _localizedPassiveSkill.StringReference =
                beast.Passive != null ? beast.Passive.Description : new LocalizedString();
            _localizedPassiveSkill.RefreshString();

            _beastElement.sprite = beast.Elemental.Icon;
        }
    }
}