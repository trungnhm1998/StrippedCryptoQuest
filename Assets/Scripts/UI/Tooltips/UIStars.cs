using UnityEngine;

namespace CryptoQuest.UI.Tooltips
{
    public class UIStars : MonoBehaviour
    {
        [SerializeField] protected GameObject[] _stars;

        public virtual void SetStars(int dataStars)
        {
            Reset();
            for (var i = 0; i < _stars.Length; i++)
            {
                _stars[i].SetActive(i < dataStars);
            }
        }

        protected virtual void Reset()
        {
            foreach (var star in _stars)
            {
                star.SetActive(false);
            }
        }
    }
}