using System.Collections;
using CryptoQuest.System.Dialogue.Events;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
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
            StartCoroutine(CoLoadTable(currentConfig));
        }

        private IEnumerator CoLoadTable(YarnProjectConfigSO config)
        {
            AsyncOperationHandle<StringTable> handle = config.StringTable.GetTableAsync();
            yield return handle;
            _dialogueRunner.yarnProject = config.YarnProject;
            _dialogueRunner.SetProject(config.YarnProject);
            _localisedLineProvider.SetStringTable(config.StringTable);
        }
    }
}