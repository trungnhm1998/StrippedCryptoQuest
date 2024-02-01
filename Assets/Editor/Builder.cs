using System;
using System.Collections.Generic;
using System.Linq;
using IndiGamesEditor.UnityBuilderAction.Input;
using IndiGamesEditor.UnityBuilderAction.Reporting;
using IndiGamesEditor.UnityBuilderAction.Versioning;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CryptoQuest.Editor
{
    static class Builder
    {
        private static string build_script = "Assets/AddressableAssetsData/DataBuilders/BuildScriptPackedMode.asset";
        private static string settings_asset = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";
        private static string profile_name = "Default";
        private static AddressableAssetSettings settingsAsset;
        private static BuildPlayerOptions buildPlayerOptions;

        private static bool ParseBuildPlayerOptions()
        {
            // Gather values from args
            var options = ArgumentsParser.GetValidatedOptions();

            // Gather values from project
            var scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(s => s.path).ToArray();

            // Get all buildOptions from options
            BuildOptions buildOptions = BuildOptions.None;
            foreach (string buildOptionString in Enum.GetNames(typeof(BuildOptions)))
            {
                if (options.ContainsKey(buildOptionString))
                {
                    BuildOptions buildOptionEnum = (BuildOptions)Enum.Parse(typeof(BuildOptions), buildOptionString);
                    buildOptions |= buildOptionEnum;
                }
            }

#if UNITY_2021_2_OR_NEWER
            // Determine subtarget
            StandaloneBuildSubtarget buildSubtarget;
            if (!options.TryGetValue("standaloneBuildSubtarget", out var subtargetValue) ||
                !Enum.TryParse(subtargetValue, out buildSubtarget))
            {
                buildSubtarget = default;
            }
#endif

            // Define BuildPlayer Options
            buildPlayerOptions = new BuildPlayerOptions
            {
#if UNITY_2021_2_OR_NEWER
                // If standalone server, build battle scene only
                scenes = (buildSubtarget == StandaloneBuildSubtarget.Server)
                    ? new string[] { "Assets/Scenes/Battle/BattleScene.unity" }
                    : scenes,
#else
                scenes = scenes,
#endif
                locationPathName = options["customBuildPath"],
                target = (BuildTarget)Enum.Parse(typeof(BuildTarget), options["buildTarget"]),
                targetGroup = (BuildTargetGroup)Enum.Parse(typeof(BuildTargetGroup), options["buildTargetGroup"]),
                options = buildOptions,
#if UNITY_2021_2_OR_NEWER
                subtarget = (int)buildSubtarget
#endif
            };

            // Set version for this build
            VersionApplicator.SetVersion(options.TryGetValue("buildVersion", out var buildVersion)
                ? buildVersion
                : VersionGenerator.Generate());
            
            if (options.TryGetValue("define", out var define))
            {
                Console.WriteLine("[CI/CD] Custom Defines: " + define.ToString());
                var symbols = new List<string>();
                symbols.AddRange(define.Split(','));

                PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.WebGL, out var allDefinesArr);

                var allDefines = new List<string>(allDefinesArr);
                allDefines.AddRange(symbols.Except(allDefines));

                PlayerSettings.SetScriptingDefineSymbols(
                    NamedBuildTarget.WebGL,
                    allDefines.ToArray());

                foreach (var item in allDefines)
                {
                    Console.WriteLine("[CI/CD] Define: " + item.ToString());
                }
            }

            // Apply Android settings
            if (buildPlayerOptions.target == BuildTarget.Android)
            {
                VersionApplicator.SetAndroidVersionCode(options["androidVersionCode"]);
                AndroidSettings.Apply(options);
            }

            // https://docs.unity3d.com/ScriptReference/EditorUserBuildSettings.SwitchActiveBuildTarget.html
            bool switchResult =
                EditorUserBuildSettings.SwitchActiveBuildTarget(buildPlayerOptions.targetGroup,
                    buildPlayerOptions.target);
            if (switchResult)
            {
                Console.WriteLine("Successfully changed Build Target to: " + buildPlayerOptions.target.ToString());
            }
            else
            {
                Debug.LogError("Unable to change Build Target to: " + buildPlayerOptions.target.ToString() +
                               " Exiting...");
                return false;
            }

            return true;
        }

        public static bool BuildAdressableData()
        {
            if (!ParseBuildPlayerOptions()) return false;

            settingsAsset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(settings_asset) as AddressableAssetSettings;
            if (settingsAsset == null)
            {
                Debug.LogError($"{settingsAsset} couldn't be found or isn't " +
                               $"a settingsAsset object.");
                return false;
            }

            string profileId = settingsAsset.profileSettings.GetProfileId(profile_name);
            if (String.IsNullOrEmpty(profileId))
                Debug.LogWarning($"Couldn't find a profile named, {profile_name}, " +
                                 $"using current profile instead.");
            else
                settingsAsset.activeProfileId = profileId;

            IDataBuilder builder = AssetDatabase.LoadAssetAtPath<ScriptableObject>(build_script) as IDataBuilder;

            if (builder == null)
            {
                Debug.LogError(build_script + " couldn't be found or isn't a build script.");
                return false;
            }

            int index = settingsAsset.DataBuilders.IndexOf((ScriptableObject)builder);
            if (index > 0)
                settingsAsset.ActivePlayerDataBuilderIndex = index;
            else
                Debug.LogWarning($"{builder} must be added to the " +
                                 $"DataBuilders list before it can be made " +
                                 $"active. Using last run builder instead.");

            AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
            bool success = string.IsNullOrEmpty(result.Error);

            if (!success)
            {
                Debug.LogError("Addressables build error encountered: " + result.Error);
            }

            return success;
        }

        public static bool BuildProject()
        {
            if (!ParseBuildPlayerOptions()) return false;

            // Perform build
            BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);

            // Summary
            BuildSummary summary = buildReport.summary;
            StdOutReporter.ReportSummary(summary);

            // Result
            BuildResult result = summary.result;
            StdOutReporter.ExitWithResult(result);
            return true;
        }
    }
}