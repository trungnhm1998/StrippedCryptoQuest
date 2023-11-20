using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.UI.Tooltips
{
    public class UIStars : MonoBehaviour
    {
        [SerializeField] private GameObject[] _stars;

        public void SetStars(int dataStars)
        {
            Reset();
            for (var i = 0; i < _stars.Length; i++)
            {
                _stars[i].SetActive(i < dataStars);
            }
        }

        private void Reset()
        {
            foreach (var star in _stars)
            {
                star.SetActive(false);
            }
        }
    }
}