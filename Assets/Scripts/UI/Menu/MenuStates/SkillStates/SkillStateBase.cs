using CryptoQuest.UI.Menu.Panels.Skill;

namespace CryptoQuest.UI.Menu.MenuStates.SkillStates
{
    /// <summary>
    /// Every state in the status menu inherits from this class.
    /// So it can have the _panels with correct type to work with.
    /// </summary>
    public abstract class SkillStateBase : MenuStateBase
    {
        protected UISkillMenu SkillPanel { get; }

        protected SkillStateBase(UISkillMenu skillPanel)
        {
            SkillPanel = skillPanel;
        }
    }
}