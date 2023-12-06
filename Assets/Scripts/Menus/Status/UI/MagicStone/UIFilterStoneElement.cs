using System;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class UIFilterStoneElement : MonoBehaviour
    {
        [SerializeField] private Button[] _navButtons;

        private void OnEnable()
        {
            _navButtons[0].Select();
        }
    }
}