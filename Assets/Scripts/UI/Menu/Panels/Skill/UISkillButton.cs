using CryptoQuest.Menu;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkillButton : MultiInputButton
    {
        protected override void Awake()
        {
            base.Awake();
            UISkillList.EnterSkillSelectionEvent += EnableButton;
            UICharacterSelection.EnterCharacterSelectionEvent += DisableButton;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UISkillList.EnterSkillSelectionEvent -= EnableButton;
            UICharacterSelection.EnterCharacterSelectionEvent -= DisableButton;
        }

        private void EnableButton()
        {
            interactable = true;
        }

        private void DisableButton()
        {
            interactable = false;
        }
    }
}
