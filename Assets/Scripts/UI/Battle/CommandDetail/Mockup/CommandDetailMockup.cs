using System;
using System.Collections.Generic;
using CryptoQuest.Battle;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Battle.CommandDetail.Mockup
{
    [Serializable]
    public class MockupButtonInfo : ButtonInfoBase
    {
        [field: SerializeField] public UnityEvent OnPressed { get; protected set; }

        public MockupButtonInfo(string label, string value = "", bool isInteractable = true)
            : base(label, value, isInteractable) { }

        public override void OnHandleClick()
        {
            Debug.Log($"Pressed {Label}");
            OnPressed?.Invoke();
        }
    }

    public class MockModel : ICommandDetailModel
    {
        private List<ButtonInfoBase> _infos;
        public List<ButtonInfoBase> Infos => _infos;
        
        public MockModel(params ButtonInfoBase[] infos)
        {
            _infos = new List<ButtonInfoBase>(infos);
        }

        public void AddInfo(params ButtonInfoBase[] infos)
        {
            _infos.AddRange(infos);
        }
    }

    public class CommandDetailMockup : MonoBehaviour
    {
        [SerializeField] private BattleStateMachine _battleStateMachine;
        [SerializeField] private MockupButtonInfo[] _buttonInfos;
        [SerializeField] private MockupButtonInfo[] _itemButtonInfos;
        [SerializeField] private MockupButtonInfo[] _skillButtonInfos;
        [SerializeField] private MockupButtonInfo[] _enemyGroupButtonInfos;

        private void Start()
        {
            ShowCommandDetail(new List<ButtonInfoBase>(_enemyGroupButtonInfos));
        }

        private void OnEnable()
        {
            CommandDetailPresenter.InspectButton += InspectingButton;
        }

        private void OnDisable()
        {
            CommandDetailPresenter.InspectButton -= InspectingButton;
        }

        private void OnEnterSelectSingleEnemyState()
        {
            ShowCommandDetail(new List<ButtonInfoBase>(_buttonInfos));
        }

        private void OnEnterSelectEnemyGroupState()
        {
            ShowCommandDetail(new List<ButtonInfoBase>(_enemyGroupButtonInfos));
        }

        private void OnEnterSelectSkillState()
        {
            ShowCommandDetail(new List<ButtonInfoBase>(_skillButtonInfos));
        }

        private void OnEnterSelectItemState()
        {
            ShowCommandDetail(new List<ButtonInfoBase>(_itemButtonInfos));
        }

        private void ShowCommandDetail(List<ButtonInfoBase> infos)
        {
            ICommandDetailModel model = new MockModel(infos.ToArray());
            CommandDetailPresenter.RequestShowCommandDetail?.Invoke(model);
        }

        private void InspectingButton(int index)
        {
            Debug.Log($"Inspecting {index}");
        }

        public void ChangeToSelectSingleEnemyState()
        {
        }

        public void ChangeToSelectGroupEnemyState()
        {
        }
    }
}