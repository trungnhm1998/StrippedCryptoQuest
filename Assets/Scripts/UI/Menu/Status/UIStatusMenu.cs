using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.UI.Menu.MockData;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Status
{
    public class UIStatusMenu : UIMenuPanel
    {
        protected override void EnablePanelInput()
        {
            _inputMediator.EnableStatusMenuInput();
            _inputMediator.StatusMenuCancelEvent += BackToNavBar;
        }

        private void BackToNavBar()
        {
            _inputMediator.StatusMenuCancelEvent -= BackToNavBar;
            PanelUnfocus?.Invoke(_typeSO);
        }
    }
}