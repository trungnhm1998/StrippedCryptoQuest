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

        public void FillUI(IBeast beast)
        {
            _beastName.StringReference = beast.LocalizedName;

            _beastPassiveSkill.StringReference =
                beast.Passive != null ? beast.Passive.Description : new LocalizedString();

            _beastElement.sprite = beast.Elemental.Icon;
        }
    }
}