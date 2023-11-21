using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Home.UI
{
    public class UIOverview : MonoBehaviour
    {
        public event UnityAction ChangeOrderEvent;
        public event UnityAction ViewCharacterListEvent;

        [SerializeField] private Button[] _overviewTabs;

        public void EnableSelectActions()
        {
            _overviewTabs[0].Select();
        }

        public void ChangOrderButtonPressed() => ChangeOrderEvent?.Invoke();
        public void CharacterListButtonPressed() => ViewCharacterListEvent?.Invoke();
    }
}