using CryptoQuest.Tavern.UI.CharacterReplacement;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Tavern.UI
{
    public class TavernPresenter : MonoBehaviour
    {
        public UnityAction ExitTavernEvent;

        [SerializeField] private UIOverview _tavernUiOverview;
        public UIOverview TavernUiOverview => _tavernUiOverview;

        [SerializeField] private TavernInputManager _tavernInputManager;
        public TavernInputManager TavernInputManager => _tavernInputManager;

        [SerializeField] private UICharacterReplacement _uiCharacterReplacement;
        public UICharacterReplacement UICharacterReplacement => _uiCharacterReplacement;
    }
}