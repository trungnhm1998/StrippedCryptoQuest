using System.Collections;
using CryptoQuest.Item.Consumable;
using CryptoQuest.ShopSystem.Sagas;
using CryptoQuest.UI.Dialogs.BattleDialog;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem.Buy.Consumable
{
    public class ShopDialogPresenter : MonoBehaviour
    {
        [SerializeField] private LocalizedString _strPresentingItems;

        private UIGenericDialog _dialog;

        private void Awake()
        {
            GenericDialogController.Instance.InstantiateAsync(dialog => _dialog = dialog);
        }

        private void OnEnable()
        {
            _dialog.WithMessage(_strPresentingItems).Show();
        }
    }
}