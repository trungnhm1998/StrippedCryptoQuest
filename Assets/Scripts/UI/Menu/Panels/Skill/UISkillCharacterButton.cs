using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkillCharacterButton : MultiInputButton
    {
        public event UnityAction<ECharacterClass> Clicked;

        [SerializeField] private ECharacterClass _typeMenuCharacter;

        private void OnClicked()
        {
            Clicked?.Invoke(_typeMenuCharacter);
        }
    }
}