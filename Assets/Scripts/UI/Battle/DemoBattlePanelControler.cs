using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public class DemoBattlePanelController : MonoBehaviour
    {
        [SerializeField] private BattlePanelController _panelController;

        [Header("Demo Panels")]
        [SerializeField] private List<ButtonInfo> _attackPanelInfo;

        [SerializeField] private List<ButtonInfo> _skillPanelInfo;
        [SerializeField] private List<ButtonInfo> _itemPanelInfo;

        private void OnEnable()
        {
            _panelController.OnButtonAttackClicked += OnButtonAttackClickedHandler;
            _panelController.OnButtonSkillClicked += OnButtonSkillClickedHandler;
            _panelController.OnButtonItemClicked += OnButtonItemClickedHandler;
        }

        private void OnDisable()
        {
            _panelController.OnButtonAttackClicked -= OnButtonAttackClickedHandler;
            _panelController.OnButtonSkillClicked -= OnButtonSkillClickedHandler;
            _panelController.OnButtonItemClicked -= OnButtonItemClickedHandler;
        }


        private void OnButtonItemClickedHandler(IBattleUnit currentUnit)
        {
            _panelController.OpenCommandDetailPanel(_itemPanelInfo);
        }

        private void OnButtonSkillClickedHandler(IBattleUnit currentUnit)
        {
            _panelController.OpenCommandDetailPanel(_skillPanelInfo);
        }

        private void OnButtonAttackClickedHandler(IBattleUnit currentUnit)
        {
            _panelController.OpenCommandDetailPanel(_attackPanelInfo);
        }
    }
}