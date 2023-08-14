using System;
using IndiGames.Core.Events.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public class UITooltip : MonoBehaviour
    {
        public delegate void TooltipEvent(Vector2 position, bool isDownWard = true);
        public static TooltipEvent ShowTooltipEvent;
        public static UnityAction HideTooltipEvent;
        [SerializeField] private RectTransform _upContainer;
        [SerializeField] private RectTransform _downContainer;
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject _frame;
        [SerializeField] private GameObject _reverseFrame;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _effectDescription;
        [SerializeField] private Image _illustration;
        [SerializeField] private Image _rarity;
        [SerializeField] private TMP_Text _level;

        private void OnEnable()
        {
            ShowTooltipEvent += ShowTooltipAtLocation;
            HideTooltipEvent += Hide;

        }

        private void OnDisable()
        {
            ShowTooltipEvent -= ShowTooltipAtLocation;
            HideTooltipEvent -= Hide;
        }

        private void ShowTooltipAtLocation(Vector2 location, bool isShowDownWard)
        {
            _downContainer.position = _upContainer.position = location;
            _content.SetActive(true);
            _reverseFrame.SetActive(isShowDownWard);
            _frame.SetActive(!isShowDownWard);
            _content.transform.parent = isShowDownWard ? _downContainer : _upContainer;
            _content.transform.localPosition = new Vector3(0, 0, 0);
        }

        private void Hide()
        {
            _content.SetActive(false);
        }
    }
}