using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Common
{
    public class SelectButtonOnEnable : MonoBehaviour
    {
        [SerializeField] private float _delay = 0.1f;
        private Button _button;

        private void Awake() => _button = GetComponent<Button>();
        private void OnEnable() => Select();

        public void Select() => Invoke(nameof(SelectButton), _delay);

        private void SelectButton() => _button.Select();
    }
}