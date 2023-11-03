using CryptoQuest.Core;
using CryptoQuest.Menu;
using UnityEngine;

namespace CryptoQuest.Menus.Settings.UI
{
    public class UILogoutButton : MonoBehaviour
    {
        [field: SerializeField] public MultiInputButton Button { get; private set; }

        public void HandleLogoutButtonClicked()
        {
            ActionDispatcher.Dispatch(new Logout());
        }
    }
}
