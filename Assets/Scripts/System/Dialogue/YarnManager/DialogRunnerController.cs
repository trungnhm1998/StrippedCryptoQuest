using CryptoQuest.System.Dialogue.Events;
using UnityEngine;
using Yarn.Unity;

namespace CryptoQuest.System.Dialogue.YarnManager
{
    public class DialogRunnerController : MonoBehaviour
    {
        [SerializeField] private DialogueRunner _dialogueRunner;
        [SerializeField] private UnityLocalisedLineProvider _localisedLineProvider;

        [Header("listeners")]
        [SerializeField] private YarnProjectConfigEvent _onYarnProjectConfigEvent;

        private void OnEnable()
        {
            _onYarnProjectConfigEvent.ConfigRequested += OnConfigRequested;
        }

        private void OnDisable()
        {
            _onYarnProjectConfigEvent.ConfigRequested -= OnConfigRequested;
        }

        private void OnConfigRequested(YarnProjectConfigSO currentConfig)
        {
            _dialogueRunner.yarnProject = currentConfig.YarnProject;
            _dialogueRunner.SetProject(currentConfig.YarnProject);
            _localisedLineProvider.StringsTable = currentConfig.StringTable;
        }
    }
}