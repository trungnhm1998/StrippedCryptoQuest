using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement;
using Yarn.Unity;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CryptoQuest
{
    public class CryptoQuestLineProvider : LineProviderBehaviour
    {
        public List<LocalizedStringTable> stringsTables = new List<LocalizedStringTable>();
        public LocalizedAssetTable assetTable;

        private List<StringTable> currentStringsTables = new();
        private AssetTable currentAssetTable;

        private List<AsyncOperationHandle<Object>> pendingLoadOperations = new();
        private Dictionary<string, Object> loadedAssets = new();

        public override string LocaleCode => LocalizationSettings.SelectedLocale.Identifier.Code;

        public override bool LinesAvailable
        {
            get => stringsTables.Count != 0 && currentStringsTables != null && (assetTable.IsEmpty ||
                (currentAssetTable != null && pendingLoadOperations.Count <= 0));
        }

        public override LocalizedLine GetLocalizedLine(Yarn.Line line)
        {
            string text = null;
            LineMetadata metadata = null;
            if (currentStringsTables != null)
            {
                foreach (var currentStringsTable in currentStringsTables)
                {
                    if (currentStringsTable[line.ID] != null)
                    {
                        text = currentStringsTable[line.ID].LocalizedValue;
                        metadata = currentStringsTable[line.ID].GetMetadata<LineMetadata>();
                        break;
                    }
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

            if (loadedAssets.TryGetValue(lineIDWithoutPrefix, out var asset))
            {
                localizedLine.Asset = asset;
            }

            return localizedLine;
        }

        private void OnEnable()
        {
            if (stringsTables != null)
            {
                stringsTables[0].TableChanged += OnStringTableChanged;
            }

            if (assetTable != null)
            {
                assetTable.TableChanged += OnAssetTableChanged;
            }
        }

        private void OnDisable()
        {
            if (stringsTables != null)
            {
                stringsTables[0].TableChanged -= OnStringTableChanged;
            }

            if (assetTable != null)
            {
                assetTable.TableChanged -= OnAssetTableChanged;
            }
        }

        private void OnStringTableChanged(StringTable value)
        {
            currentStringsTables.Clear();
            foreach (LocalizedStringTable stringsTable in stringsTables)
            {
                currentStringsTables.Add(stringsTable.GetTable());
            }
        }

        private void OnAssetTableChanged(AssetTable value)
        {
            currentAssetTable = value;
        }

        public override void PrepareForLines(IEnumerable<string> lineIDs)
        {
            if (assetTable.IsEmpty != true)
            {
                if (currentAssetTable == null)
                {
                    RunAfterComplete(assetTable.GetTableAsync(),
                        (loadedAssetTable) => { PreloadLinesFromTable(loadedAssetTable, lineIDs); });
                }
                else
                {
                    PreloadLinesFromTable(currentAssetTable, lineIDs);
                }
            }

            void PreloadLinesFromTable(AssetTable table, IEnumerable<string> lineIDs)
            {
                var lineIDsWithoutPrefix = new List<string>();
                foreach (var l in lineIDs)
                {
                    lineIDsWithoutPrefix.Add(l.Replace("line:", ""));
                }

                var assetKeysToUnload = new HashSet<string>(loadedAssets.Keys);
                assetKeysToUnload.ExceptWith(lineIDsWithoutPrefix);
                foreach (var assetKeyToUnload in assetKeysToUnload)
                {
                    var entryToRelease = table.GetEntry(assetKeyToUnload);

                    if (entryToRelease != null)
                    {
                        table.ReleaseAsset(entryToRelease);

                        loadedAssets.Remove(assetKeyToUnload);
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
                        loadedAssets[id] = loadOperation.Result;
                    }
                    else
                    {
                        pendingLoadOperations.Add(loadOperation);
                        loadOperation.Completed += (operation) =>
                        {
                            pendingLoadOperations.Remove(loadOperation);
                            if (operation.Status == AsyncOperationStatus.Succeeded)
                            {
                                loadedAssets[id] = operation.Result;
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
    [CustomEditor(typeof(CryptoQuestLineProvider))]
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
            var stringsTablesProperty = serializedObject.FindProperty(nameof(CryptoQuestLineProvider.stringsTables));
            stringsTableProperties = stringsTablesProperty;
            this.assetTableProperty = serializedObject.FindProperty(nameof(CryptoQuestLineProvider.assetTable));
        }
    }
#endif
}