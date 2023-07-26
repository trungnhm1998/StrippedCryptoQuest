using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using Yarn.Unity;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CryptoQuest.DialogueSystem
{
    public class MultipleLocalizationLineProvider : LineProviderBehaviour
    {
        [FormerlySerializedAs("stringsTables")]
        public List<LocalizedStringTable> StringsTables = new();

        [FormerlySerializedAs("assetTable")]
        public LocalizedAssetTable AssetTable;

        private List<StringTable> _currentStringsTables = new();
        private AssetTable _currentAssetTable;

        private List<AsyncOperationHandle<Object>> _pendingLoadOperations = new();
        private Dictionary<string, Object> _loadedAssets = new();

        public override string LocaleCode => LocalizationSettings.SelectedLocale.Identifier.Code;

        public override bool LinesAvailable
        {
            get => StringsTables.Count != 0 && _currentStringsTables != null && (AssetTable.IsEmpty ||
                (_currentAssetTable != null && _pendingLoadOperations.Count <= 0));
        }

        public override LocalizedLine GetLocalizedLine(Yarn.Line line)
        {
            string text = null;
            LineMetadata metadata = null;
            if (_currentStringsTables != null)
            {
                foreach (var currentStringsTable in _currentStringsTables)
                {
                    if (currentStringsTable == null || currentStringsTable[line.ID] == null)
                        continue;

                    text = currentStringsTable[line.ID].LocalizedValue;
                    metadata = currentStringsTable[line.ID].GetMetadata<LineMetadata>();
                    break;
                }
            }

            if (text == null)
                text = $"Error: Missing localisation for line {line.ID} in string tables";

            LocalizedLine localizedLine = new LocalizedLine()
            {
                TextID = line.ID,
                RawText = text,
                Substitutions = line.Substitutions,
            };

            if (metadata != null)
            {
                localizedLine.Metadata = metadata.tags;
            }

            var lineIDWithoutPrefix = line.ID.Replace("line:", "");

            if (_loadedAssets.TryGetValue(lineIDWithoutPrefix, out var asset))
            {
                localizedLine.Asset = asset;
            }

            return localizedLine;
        }

        private void OnEnable()
        {
            if (StringsTables != null)
            {
                StringsTables[0].TableChanged += OnStringTableChanged;
            }

            if (AssetTable != null)
            {
                AssetTable.TableChanged += OnAssetTableChanged;
            }
        }

        private void OnDisable()
        {
            if (StringsTables != null)
            {
                StringsTables[0].TableChanged -= OnStringTableChanged;
            }

            if (AssetTable != null)
            {
                AssetTable.TableChanged -= OnAssetTableChanged;
            }
        }

        private void OnStringTableChanged(StringTable value)
        {
            _currentStringsTables.Clear();
            foreach (LocalizedStringTable stringsTable in StringsTables)
            {
                _currentStringsTables.Add(stringsTable.GetTable());
            }
        }

        private void OnAssetTableChanged(AssetTable value)
        {
            _currentAssetTable = value;
        }

        public override void PrepareForLines(IEnumerable<string> lineIDs)
        {
            if (AssetTable.IsEmpty != true)
            {
                if (_currentAssetTable == null)
                {
                    RunAfterComplete(AssetTable.GetTableAsync(),
                        (loadedAssetTable) => { PreloadLinesFromTable(loadedAssetTable, lineIDs); });
                }
                else
                {
                    PreloadLinesFromTable(_currentAssetTable, lineIDs);
                }
            }

            void PreloadLinesFromTable(AssetTable table, IEnumerable<string> lineIDs)
            {
                var lineIDsWithoutPrefix = new List<string>();
                foreach (var l in lineIDs)
                {
                    lineIDsWithoutPrefix.Add(l.Replace("line:", ""));
                }

                var assetKeysToUnload = new HashSet<string>(_loadedAssets.Keys);
                assetKeysToUnload.ExceptWith(lineIDsWithoutPrefix);
                foreach (var assetKeyToUnload in assetKeysToUnload)
                {
                    var entryToRelease = table.GetEntry(assetKeyToUnload);

                    if (entryToRelease != null)
                    {
                        table.ReleaseAsset(entryToRelease);

                        _loadedAssets.Remove(assetKeyToUnload);
                    }
                }

                foreach (var id in lineIDsWithoutPrefix)
                {
                    var entry = table.GetEntry(id);
                    if (entry == null)
                    {
                        continue;
                    }

                    var loadOperation = table.GetAssetAsync<Object>(entry.KeyId);

                    if (loadOperation.IsDone == false)
                    {
                        Debug.Log($"Asset for {id} was already loaded");
                        _loadedAssets[id] = loadOperation.Result;
                    }
                    else
                    {
                        _pendingLoadOperations.Add(loadOperation);
                        loadOperation.Completed += (operation) =>
                        {
                            _pendingLoadOperations.Remove(loadOperation);
                            if (operation.Status == AsyncOperationStatus.Succeeded)
                            {
                                _loadedAssets[id] = operation.Result;
                            }
                            else
                            {
                                Debug.LogError($"Asset load operation for ID {id} failed!");
                            }
                        };
                    }
                }
            }
        }

        private void RunAfterComplete<T>(AsyncOperationHandle<T> operation, Action<T> onComplete,
            Action onFailure = null)
        {
            if (onComplete is null) { }

            StartCoroutine(RunAfterCompleteImpl(operation, onComplete));

            IEnumerator RunAfterCompleteImpl(AsyncOperationHandle<T> operation, Action<T> onComplete)
            {
                yield return operation;

                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    onComplete(operation.Result);
                }
                else
                {
                    onFailure?.Invoke();
                }
            }
        }
    }

    public class LineMetadata : UnityEngine.Localization.Metadata.IMetadata
    {
        public string nodeName;
        public string[] tags;
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(MultipleLocalizationLineProvider))]
    public class CryptoQuestLocalisedLineProviderEditor : Editor
    {
        private SerializedProperty stringsTableProperties;
        private SerializedProperty assetTableProperty;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(stringsTableProperties);
            EditorGUILayout.PropertyField(assetTableProperty);
            serializedObject.ApplyModifiedProperties();
        }

        public void OnEnable()
        {
            var stringsTablesProperty =
                serializedObject.FindProperty(nameof(MultipleLocalizationLineProvider.StringsTables));
            stringsTableProperties = stringsTablesProperty;
            this.assetTableProperty =
                serializedObject.FindProperty(nameof(MultipleLocalizationLineProvider.AssetTable));
        }
    }
#endif
}