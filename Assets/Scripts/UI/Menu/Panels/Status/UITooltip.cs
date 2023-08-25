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
        public delegate void TooltipEvent(Vector2 upwardPosition, Vector2 downwardPosition, float rectHeight);
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
        private bool _isShowTooltip = false;

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


        private void SetTooltipLocation(Vector2 upwardTooltipPoint, Vector2 downwardTooltipPoint, float rectHeight)
        {
            _isShowTooltip = true;
            _content.SetActive(false);
            _upContainer.transform.position = downwardTooltipPoint;
            _downContainer.transform.position = upwardTooltipPoint;
            _isShowDownWard = (_upContainer.localPosition.y <= -rectHeight);
            _content.transform.parent = _isShowDownWard ? _downContainer : _upContainer;
            _content.transform.localPosition = new Vector3(0, 0, 0);
            StartCoroutine(ShowTooltip());
        }

        private IEnumerator ShowTooltip()
        {
            yield return new WaitForSeconds(_waitBeforePopupTooltip);
            if (_isShowTooltip)
            {
                _reverseFrame.SetActive(_isShowDownWard);
                _frame.SetActive(!_isShowDownWard);
                _content.SetActive(true);
            }
        }

        private void Hide()
        {
            _isShowTooltip = false;
            _content.SetActive(false);
        }
    }
}