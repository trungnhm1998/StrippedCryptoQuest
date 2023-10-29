using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using IndiGamesEditor.UnityBuilderAction.Versioning;
#endif

namespace IndiGames.Core.UI
{
    public class Versioning : MonoBehaviour
    {
        [SerializeField] private Text _versionText;

        void Start()
        {
            _versionText.text = $"v{Application.version}";

#if UNITY_EDITOR
            _versionText.text = $"v{VersionGenerator.Generate()}";
#endif
        }
    }
}