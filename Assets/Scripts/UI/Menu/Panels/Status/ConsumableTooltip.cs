using CryptoQuest.Item;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class ConsumableTooltip : UITooltip
    {
        [SerializeField] private LocalizeStringEvent _displayNameString;
        public override ITooltip WithHeader(LocalizedString dataDisplayName)
        {
            _displayNameString.StringReference = dataDisplayName;
            return this;
        }
    }
}
