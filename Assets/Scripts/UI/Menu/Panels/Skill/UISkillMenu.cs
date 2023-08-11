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
        [Header("State Context")]
        [SerializeField] private UISkillManager _characterSelectionOverview;
        public UISkillManager CharacterSelectionOverview => _characterSelectionOverview;

        [SerializeField] private UISkillManager _skillSelectionOverview;
        public UISkillManager SkillSelectionOverview => _skillSelectionOverview;

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
    }
}