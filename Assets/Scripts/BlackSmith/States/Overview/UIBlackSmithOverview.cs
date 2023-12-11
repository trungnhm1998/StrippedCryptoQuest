using System;
using UnityEngine;

namespace CryptoQuest.BlackSmith.States.Overview
{
    public class UIBlackSmithOverview : MonoBehaviour
    {
        public event Action OpenUpgrading;
        public event Action OpenEvolving;
        public event Action OpenStoneUpgrading;

        public void OnUpgradeButtonPressed() => OpenUpgrading?.Invoke();

        public void OnEvolveButtonPressed() => OpenEvolving?.Invoke();

        public void OnStoneUpgradeButtonPressed() => OpenStoneUpgrading?.Invoke();
    }
}