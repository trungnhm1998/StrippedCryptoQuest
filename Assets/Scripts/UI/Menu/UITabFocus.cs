using TMPro;
using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    public class UITabFocus : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Color _unfocus = Color.gray;
        [SerializeField] private Color _focus = Color.white;

        public void Focus()
        {
            _text.color = _focus;
        }

        public void UnFocus()
        {
            _text.color = _unfocus;
        }
    }
}