﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IndiGamesEditor.UnityBuilderAction.Input;
using IndiGamesEditor.UnityBuilderAction.Reporting;
using IndiGamesEditor.UnityBuilderAction.Versioning;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace IndiGamesEditor.UnityBuilderAction
{
    public static class Builder
    {
        public static void BuildProject()
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
            var buildPlayerOptions = new BuildPlayerOptions
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
                options = buildOptions,
#if UNITY_2021_2_OR_NEWER
                subtarget = (int)buildSubtarget
#endif
            };

            BuildVersion(options, buildPlayerOptions);

            BuildAddressable();

            // Perform build
            BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);

            // Summary
            BuildSummary summary = buildReport.summary;
            StdOutReporter.ReportSummary(summary);

            // Result
            BuildResult result = summary.result;
            StdOutReporter.ExitWithResult(result);
        }

        private static void BuildVersion(Dictionary<string, string> options, BuildPlayerOptions buildPlayerOptions)
        {
            // Set version for this build
            VersionApplicator.SetVersion(options.TryGetValue("buildVersion", out var buildVersion)
                ? buildVersion
                : VersionGenerator.Generate());


            // Apply Android settings
            if (buildPlayerOptions.target == BuildTarget.Android)
            {
                VersionApplicator.SetAndroidVersionCode(options["androidVersionCode"]);
                AndroidSettings.Apply(options);
            }

            VersionApplicator.SetVersion(VersionGenerator.Generate());
        }

        public static void BuildAddressable()
        {
            Debug.Log($"VERSION: {VersionGenerator.Generate()}");
            
            // Execute default AddressableAsset content build, if the package is installed.
            // Version defines would be the best solution here, but Unity 2018 doesn't support that,
            // so we fall back to using reflection instead.
            var addressableAssetSettingsType = Type.GetType(
                "UnityEditor.AddressableAssets.Settings.AddressableAssetSettings,Unity.Addressables.Editor");
            if (addressableAssetSettingsType != null)
            {
                // ReSharper disable once PossibleNullReferenceException, used from try-catch
                try
                {
                    addressableAssetSettingsType
                        .GetMethod("CleanPlayerContent", BindingFlags.Static | BindingFlags.Public)
                        .Invoke(null, new object[] { null });
                    addressableAssetSettingsType.GetMethod("BuildPlayerContent", new Type[0])
                        .Invoke(null, new object[0]);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to run default addressables build:\n{e}");
                }
            }
        }
    }
}