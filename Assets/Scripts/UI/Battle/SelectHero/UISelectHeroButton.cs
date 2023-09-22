using System;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Menu;
using CryptoQuest.UI.Battle.CommandDetail;
using CryptoQuest.UI.Battle.PlayerParty;
using CryptoQuest.UI.Battle.StateMachine;
using FSM;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle.SelectHero
{
    [RequireComponent(typeof(MultiInputButton))]
    public class UISelectHeroButton : MonoBehaviour, IBattleMenu
    {
        public event Action ConfirmPressed;

        public static readonly string SelectHeroState = "SelectHero";
        public string StateName => SelectHeroState;

        [SerializeField] private MultiInputButton _button;
        [SerializeField] private GameObject _content;
        [SerializeField] private LocalizeStringEvent _label;

        public StateBase<string> CreateState(BattleMenuStateMachine machine)
        {
            return new SelectHeroState(machine, this);;
        }

        private void OnValidate()
        {
            _button = GetComponent<MultiInputButton>();
        }

        private void Awake()
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(ConfirmCharacter);
        }

        public void SetUIActive(bool value)
        {
            _button.interactable = value;
            _content.SetActive(value);
        }

        public void SelectButton()
        {
            _button.Select();
        }

        public void SetLabel(LocalizedString label)
        {
            if (label == null) return;
            _label.StringReference = label;
        }

        public void SetUIPosition(Vector3 position)
        {
            transform.position = position;
        }

        private void ConfirmCharacter()
        {
            ConfirmPressed?.Invoke();
        }
    }
}