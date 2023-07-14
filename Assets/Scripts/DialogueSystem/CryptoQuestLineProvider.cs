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
        [SerializeField] internal List<LocalizedStringTable> stringsTables = new List<LocalizedStringTable>();
        [SerializeField] internal LocalizedAssetTable assetTable;

        private List<StringTable> currentStringsTables = new List<StringTable>();
        private AssetTable currentAssetTable;

        private List<AsyncOperationHandle<Object>> pendingLoadOperations = new List<AsyncOperationHandle<Object>>();
        private Dictionary<string, Object> loadedAssets = new Dictionary<string, Object>();

        public override string LocaleCode => LocalizationSettings.SelectedLocale.Identifier.Code;

        public override bool LinesAvailable
        {
            get
            {
                if (this.stringsTables.Count == 0)
                {
                    return false;
                }

                if (this.currentStringsTables == null)
                {
                    return false;
                }

                if (this.assetTable.IsEmpty == false)
                {
                    if (this.currentAssetTable == null)
                    {
                        return false;
                    }

                    if (pendingLoadOperations.Count > 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public override LocalizedLine GetLocalizedLine(Yarn.Line line)
        {
            var text = line.ID;
            if (currentStringsTables != null)
            {
                foreach (var currentStringsTable in currentStringsTables)
                {
                    if (currentStringsTable[line.ID] == null)
                    {
                        text =
                            $"Error: Missing localisation for line {line.ID} in string table {currentStringsTable.LocaleIdentifier}";
                    }
                    else
                    {
                        text = currentStringsTable[line.ID].LocalizedValue;
                        break;
                    }
                }
            }

            // Construct the localized line
            LocalizedLine localizedLine = new LocalizedLine()
            {
                TextID = line.ID,
                RawText = text,
                Substitutions = line.Substitutions,
            };
            LineMetadata metadata = null;
            foreach (var currentStringsTable in currentStringsTables)
            {
                if (currentStringsTable[line.ID] != null)
                {
                    metadata = currentStringsTable[line.ID].GetMetadata<LineMetadata>();
                    break;
                }
            }

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

        public override void Start()
        {
            if (stringsTables != null)
            {
                OnStringTableChanged();
            }

            if (assetTable != null)
            {
                assetTable.TableChanged += OnAssetTableChanged;
            }
        }

        private void OnStringTableChanged()
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