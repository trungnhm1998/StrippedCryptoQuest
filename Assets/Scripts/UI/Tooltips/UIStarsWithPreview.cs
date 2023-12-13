using System;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Tooltips
{
    public class UIStarsWithPreview : UIStars
    {
        [SerializeField] private Sprite _previewStar;
        [SerializeField] private Sprite _originalStar;

        private Image[] _starsImage;

        private void Init()
        {
            _starsImage = new Image[_stars.Length];
            for (var i = 0; i < _stars.Length; i++)
            {
                _starsImage[i] = _stars[i].GetComponent<Image>();
            }
        }

        public override void SetStars(int dataStars)
        {
            Reset();
            for (var i = 0; i < _stars.Length; i++)
            {
                if (i < dataStars)
                {
                    _starsImage[i].sprite = _originalStar;
                    _starsImage[i].gameObject.SetActive(true);
                }
            }
        }

        public void SetPreviewStars(int min, int max)
        {
            for (var i = 0; i < _stars.Length; i++)
            {
                if (i >= min && i < max)
                {
                    _starsImage[i].sprite = _previewStar;
                    _starsImage[i].gameObject.SetActive(true);
                }
            }
        }

        public void HidePreview() => Reset();

        protected override void Reset()
        {
            if (_starsImage == null || _starsImage.Length == 0)
                Init(); // TODO: Find a way to init later since Awake doesn't initialize the stars on time.

            for (var i = 0; i < _starsImage.Length; i++)
            {
                _starsImage[i].gameObject.SetActive(false);
                _starsImage[i].sprite = _originalStar;
            }
        }
    }
}