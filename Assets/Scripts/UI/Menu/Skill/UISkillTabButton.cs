using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Skill
{
    public class UISkillTabButton : MonoBehaviour
    {
        public event UnityAction<ECharacterClass> Clicked;

        [SerializeField] private ECharacterClass _typeMenuCharacter;

        private void OnClicked()
        {
            Clicked?.Invoke(_typeMenuCharacter);
        }

        public void Select()
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}
