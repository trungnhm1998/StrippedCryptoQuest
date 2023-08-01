using System;
using System.Collections.Generic;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Status
{
    public class UIMagicStoneMenu : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private GameObject _contents;

        public void ShowPanel()
        {
            _contents.SetActive(true);
            _inputMediator.EnableStatusMagicStoneInput();

            _inputMediator.TurnOffMagicStoneMenuEvent += HidePanel;
        }
        
        private void HidePanel()
        {
            _contents.SetActive(false);
            _inputMediator.EnableStatusEquipmentsInput();

            _inputMediator.TurnOffMagicStoneMenuEvent -= HidePanel;
        }
    }
}