using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

namespace CryptoQuest.UI.Battle.CommandDetail.Mockup
{
    [Serializable]
    public class MockupButtonInfo : ButtonInfoBase
    {
        public MockupButtonInfo(string label, string value = "", bool isInteractable = true)
            : base(label, value, isInteractable)
        {
        }

        public override void OnHandleClick()
        {
            Debug.Log($"Pressed {Label}");
        }
    }

    public class CommandDetailMockupPresenter : MonoBehaviour
    {
        [SerializeField] private MockupButtonInfo[] _buttonInfos;

        private void Start()
        {
            var buttons = new List<ButtonInfoBase>(_buttonInfos);
            UICommandDetailPanel.RequestShowCommandDetail?.Invoke(buttons);
        }

        private void OnEnable()
        {
            UICommandDetailButton.InspectingButton += InspectingButton;
        }

        private void OnDisable()
        {
            UICommandDetailButton.InspectingButton -= InspectingButton;
        }

        private void InspectingButton(int index)
        {
            Debug.Log($"Inspecting {index}");
        }
    }
}