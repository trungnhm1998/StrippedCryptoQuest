using CryptoQuest.Battle.UI.StartBattle;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class IntroPromptMessageController : MonoBehaviour
    {
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private UIIntroBattle _uiIntroBattle;

        private void Awake()
        {
            ConfigureIntroMessage();
        }

        private void ConfigureIntroMessage()
        {
            if (_battleBus.CurrentBattlefield == null) return;
            bool isPromptEmpty = _battleBus.CurrentBattlefield.BattlefieldPrompts.IntroPrompt.IsEmpty;
            _uiIntroBattle.IntroMessage = isPromptEmpty
                ? _uiIntroBattle.IntroMessage
                : _battleBus.CurrentBattlefield.BattlefieldPrompts.IntroPrompt;
        }
    }
}