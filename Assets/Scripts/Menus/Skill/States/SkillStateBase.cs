using CryptoQuest.Menus.Skill.UI;
using FSM;

namespace CryptoQuest.Menus.Skill.States
{
    /// <summary>
    /// Every state in the status menu inherits from this class.
    /// So it can have the _panels with correct type to work with.
    /// </summary>
    public abstract class SkillStateBase : StateBase
    {
        protected UISkillMenu SkillPanel { get; }

        protected SkillStateBase(UISkillMenu skillPanel) : base(false)
        {
            SkillPanel = skillPanel;
        }
    }
}