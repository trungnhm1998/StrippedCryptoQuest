using CryptoQuest.Input;
using CryptoQuest.UI.Menu.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Menu
{
    public abstract class UIMenuPanel : MonoBehaviour
    {
        [FormerlySerializedAs("typeSo")] [SerializeField] protected MenuTypeSO _typeSO;
        public MenuTypeSO TypeSO => _typeSO;

        public UnityAction<MenuTypeSO> PanelUnfocus;

        [SerializeField] private GameObject _content;
        [SerializeField] protected InputMediatorSO _inputMediator;

        public void Show()
        {
            _content.SetActive(true);
            EnablePanelInput();
        }

        public void Hide()
        {
            _content.SetActive(false);
        }

        protected abstract void EnablePanelInput();
    }
}