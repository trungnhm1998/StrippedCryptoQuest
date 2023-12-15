using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace CryptoQuest.Ranch.Evolve.UI
{
    public class UIBeastEvolveResultTitle : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _localizeTitle;
        
        public void ShowResultTitle(LocalizedString localized)
        {
            _localizeTitle.StringReference = localized;
        }
    }
}