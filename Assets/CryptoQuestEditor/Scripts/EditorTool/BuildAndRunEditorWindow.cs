using System.Diagnostics;
using System.IO;
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
        private const string FIRST_SCENE_PATH = "Assets/Scenes/Startup.unity";
        private const string SERVER_LOCAL_NAME = "ServerLocal.bat";

        private const string DISABLE_ECHO = "echo off";
        private const string CHANGE_DIRECTORY = "cd";
        private const string START_LOCALHOST = "start /max \"\"\"\" http://localhost";
        private const string START_PYTHON_HTTP_SERVER = "python -m http.server 80";

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

            // Prevent domain reload during critical sections
            EditorApplication.LockReloadAssemblies();

            try
            {
                if (!BuildAddressable()) return;
                if (!Build(path)) return;
                CreateBatFileAndRun(path);
            }
            finally
            {
                // Allow domain reload after critical sections
                EditorApplication.UnlockReloadAssemblies();
            }
        }

        private void CreateBatFileAndRun(string path)
        {
            CreateBatFile(path);

            string fullPath = $"{path}/{SERVER_LOCAL_NAME}";

            if (!File.Exists(fullPath))
            {
                Debug.LogWarning(
                    $"<color=red>[CreateBatFileAndRun]</color> {SERVER_LOCAL_NAME} file not found at {fullPath}i.");
                return;
            }

            Process.Start($"{fullPath}");
        }

        private void CreateBatFile(string path)
        {
            StreamWriter writer = new StreamWriter($"{path}/{SERVER_LOCAL_NAME}", false);

            writer.WriteLine(DISABLE_ECHO);
            writer.WriteLine($"{CHANGE_DIRECTORY} {path}");
            writer.WriteLine(START_LOCALHOST);
            writer.WriteLine(START_PYTHON_HTTP_SERVER);
            writer.Close();
        }

        private bool Build(string path)
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] { FIRST_SCENE_PATH },
                locationPathName = path,
                target = EditorUserBuildSettings.activeBuildTarget,
                options = EditorUserBuildSettings.development
                    ? BuildOptions.Development | BuildOptions.AllowDebugging
                    : BuildOptions.None,
                targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup,
            };

            PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded) return true;
            Debug.LogWarning($"<color=red>[Build]</color> Build failed: {summary.result}");

            foreach (var step in report.steps)
            {
                if (summary.result != BuildResult.Failed) continue;
                Debug.LogWarning($"<color=red>[Build]</color> Step '{step.name}' failed: {step.messages}");
            }

            return false;
        }

        private bool BuildAddressable()
        {
            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
            bool success = string.IsNullOrEmpty(result.Error);

            if (success) return true;
            Debug.Log($"<color=red>[BuildAddressable]</color> Build error: {result.Error}");

            return false;
        }
    }
}