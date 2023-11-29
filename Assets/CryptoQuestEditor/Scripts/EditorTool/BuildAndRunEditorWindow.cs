using System.Diagnostics;
using System.IO;
using CryptoQuestEditor.EditorTool;
using IndiGamesEditor.UnityBuilderAction.Versioning;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CryptoQuestEditor.CryptoQuestEditor.Scripts.EditorTool
{
    public class BuildAndRunEditorWindow : EditorWindow
    {
        const string application = @"cmd.exe";

        private const string FIRST_SCENE_PATH = "Assets/Scenes/Startup.unity";
        private const string SERVER_LOCAL_NAME = "ServerLocal.bat";

        [MenuItem("Crypto Quest/Build and Run %#b")]
        public static void BuildAndRun()
        {
            var window = GetWindow<BuildAndRunEditorWindow>();
            window.Close();
        }

        private void OnEnable()
        {
            string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
            if (path.Length == 0) return;

            if (!BuildAddressable()) return;

            if (!Build(path)) return;

            Debug.Log($"<color=green>[Build]</color> Build success: {path}");

            CreateBatFileAndRun(path);
        }

        private void CreateBatFileAndRun(string path)
        {
            CreateBatFile(path);

            string fullPath = $"{path}/{SERVER_LOCAL_NAME}";

            if (!File.Exists(fullPath))
            {
                Debug.LogError($"{SERVER_LOCAL_NAME} file not found at the specified location.");
                return;
            }

            Process.Start($"{fullPath}");
        }

        private void CreateBatFile(string path)
        {
            StreamWriter writer = new StreamWriter($"{path}/{SERVER_LOCAL_NAME}", false);

            writer.WriteLine("echo off");
            writer.WriteLine($"cd {path}");
            writer.WriteLine("start /max \"\"\"\" http://localhost");
            writer.WriteLine("python -m http.server 80");
            writer.Close();
        }

        private bool Build(string path)
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] { FIRST_SCENE_PATH },
                locationPathName = path,
                target = BuildTarget.WebGL,
                options = BuildOptions.Development | BuildOptions.AllowDebugging,
                targetGroup = BuildTargetGroup.WebGL
            };

            BuildPipeline.BuildPlayer(buildPlayerOptions);

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            switch (summary.result)
            {
                case BuildResult.Failed:
                    Debug.Log($"<color=red>[Build]</color> Build error: {summary.totalErrors}");
                    return false;
                case BuildResult.Cancelled:
                    Debug.Log($"<color=yellow>[Build]</color> Build stop: {summary.totalErrors}");
                    return false;
                case BuildResult.Unknown:
                    Debug.Log($"<color=purple>[Build]</color> Build unknown: {summary.totalErrors}");
                    return false;
                case BuildResult.Succeeded:
                default:
                    return true;
            }
        }

        private bool BuildAddressable()
        {
            AddressableAssetSettings
                .BuildPlayerContent(out AddressablesPlayerBuildResult result);
            bool success = string.IsNullOrEmpty(result.Error);

            if (!success)
            {
                Debug.Log($"<color=red>[Addressables]</color> Build error: {result.Error}");
            }

            return success;
        }
    }
}