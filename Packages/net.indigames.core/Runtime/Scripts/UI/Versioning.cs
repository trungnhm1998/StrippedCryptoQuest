using UnityEngine;
using UnityEngine.UI;

namespace IndiGames.Core.UI
{
    public class Versioning : MonoBehaviour
    {
        [SerializeField] private Text _versionText;
        void Start()
        {
            _versionText.text = $"v{Application.version}";
        }
    }
}