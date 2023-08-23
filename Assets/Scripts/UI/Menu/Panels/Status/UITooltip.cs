using System;
using System.Collections;
using System.Collections.Generic;
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
        public delegate void TooltipEvent(Vector2 upwardPosition, Vector2 downwardPosition);
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
        [SerializeField] private float _waitBeforePopupTooltip;
        private bool _isShowDownWard;

        private void OnEnable()
        {
            ShowTooltipEvent += SetTooltipLocation;
            HideTooltipEvent += Hide;
        }

        private void OnDisable()
        {
            ShowTooltipEvent -= SetTooltipLocation;
            HideTooltipEvent -= Hide;
        }

        private void SetTooltipLocation(Vector2 upwardTooltipPoint, Vector2 downwardTooltipPoint)
        {
            _content.SetActive(false);
            if (_upContainer.localPosition.y <= 0) _isShowDownWard = true;
            else _isShowDownWard = false;
            _upContainer.transform.position = downwardTooltipPoint;
            _downContainer.transform.position = upwardTooltipPoint;
            _content.transform.parent = _isShowDownWard ? _downContainer : _upContainer;
            _content.transform.localPosition = new Vector3(0, 0, 0);
            StartCoroutine(ShowTooltip());
        }

        private IEnumerator ShowTooltip()
        {
            yield return new WaitForSeconds(_waitBeforePopupTooltip);
            _reverseFrame.SetActive(_isShowDownWard);
            _frame.SetActive(!_isShowDownWard);
            _content.SetActive(true);
        }

        private void Hide()
        {
            _content.SetActive(false);
        }
    }
}