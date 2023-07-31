using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Skill
{
    public class UISkillTabButton : MonoBehaviour
    {
        public event UnityAction<ECharacterClasses> Clicked;

        [SerializeField] private ECharacterClasses _typeMenuCharacter;

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
