using CryptoQuest.Networking;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.Networking
{
    [CustomEditor(typeof(Credentials))]
    public class CredentialsEditor : Editor
    {
        private TextField _urlTextField;
        private Credentials Target => target as Credentials;

        public override VisualElement CreateInspectorGUI()
        {
            // render defaults
            var root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);


            var clearButton = new Button(() => PlayerPrefs.DeleteAll())
            {
                text = "Clear"
            };
            root.Add(clearButton);

            _urlTextField = new TextField("URL")
            {
                value = "https://games.indigames.link/crypto-quest/dev/"
            };
            root.Add(_urlTextField);

            var loginOnWebButton = new Button(() => { Application.OpenURL(_urlTextField.value); })
            {
                text = "Login"
            };
            root.Add(loginOnWebButton);

            return root;
        }
    }
}