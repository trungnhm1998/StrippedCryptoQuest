using DG.Tweening;
using UnityEngine;

namespace CryptoQuest.UI.Title
{
    public class UILoadingPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _loadingIcon;
        [SerializeField] private float _rotateSpeed = 1f;
        [SerializeField] private Vector3 _angle = new Vector3(0, 0, 359f);
        private float _startTime;

        private void Start()
        {
            StartLoading();
        }

        private void StartLoading()
        {
            _loadingIcon.transform.DORotate(_angle, _rotateSpeed, RotateMode.Fast)
                .SetRelative(true)
                .SetLoops(-1)
                .SetEase(Ease.Linear);
        }
    }
}