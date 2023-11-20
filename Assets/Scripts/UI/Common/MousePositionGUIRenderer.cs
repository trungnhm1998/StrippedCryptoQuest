using UnityEngine;

namespace CryptoQuest.UI.Common
{
    public class MousePositionGUIRenderer : MonoBehaviour
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private void OnGUI()
        {
            var mousePosition = UnityEngine.Input.mousePosition;
            GUI.Label(new Rect(mousePosition.x, Screen.height - mousePosition.y, 100, 100), $"Mouse: {mousePosition}");
        }
#endif
    }
}