using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.MenuStates.SkillStates;
using FSM;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    /// <summary>
    /// The context that hopefully holds all the UI information for the Skill Menu. This is a mono behaviour class that
    /// can controls all the UI element or at least delegate back the reference to the correct state when needed.
    /// </summary>
    public class UISkillMenu : UIMenuPanel
    {
        /// <summary>
        /// Return the specific state machine for this panel.
        /// </summary>
        /// <param name="menuManager"></param>
        /// <returns>The <see cref="SkillMenuStateMachine"/> which derived
        /// <see cref="CryptoQuest.UI.Menu.MenuStates.MenuStateMachine"/> derived
        /// from <see cref="StateMachine"/> which also derived from <see cref="StateBase"/></returns>
        public override StateBase<string> GetPanelState(MenuManager menuManager)
        {
            return new SkillMenuStateMachine(this);
        }

        [SerializeField] private ServiceProvider _provider;

        [Header("State Context")]
        [SerializeField] private UICharacterSelection _charactersPanel;
        public UICharacterSelection CharactersPanel => _charactersPanel;

        private IParty _party;

        private void Awake()
        {
            _party = _provider.PartyController.Party;
        }
    }
}