using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class InjectSystemVariables
{
    const string DUMMY_VALUE = "000000000";

    // Just to register Quit() function
    static InjectSystemVariables()
    {
        EditorApplication.quitting += OnEditorClosed;
    }

    // Inject system variables on project loaded
    [InitializeOnLoadMethod]
    static void OnProjectLoadedInEditor()
    {
        PlayerSettings.SetTemplateCustomValue("FIREBASE_API_KEY", System.Environment.GetEnvironmentVariable("FIREBASE_API_KEY"));
        PlayerSettings.SetTemplateCustomValue("FIREBASE_AUTH_DOMAIN", System.Environment.GetEnvironmentVariable("FIREBASE_AUTH_DOMAIN"));
        PlayerSettings.SetTemplateCustomValue("FIREBASE_PROJECT_ID", System.Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID"));
        PlayerSettings.SetTemplateCustomValue("FIREBASE_STORAGE_BUCKET", System.Environment.GetEnvironmentVariable("FIREBASE_STORAGE_BUCKET"));
        PlayerSettings.SetTemplateCustomValue("FIREBASE_MESSAGE_SENDER_ID", System.Environment.GetEnvironmentVariable("FIREBASE_MESSAGE_SENDER_ID"));
        PlayerSettings.SetTemplateCustomValue("FIREBASE_APP_ID", System.Environment.GetEnvironmentVariable("FIREBASE_APP_ID"));
        PlayerSettings.SetTemplateCustomValue("FIREBASE_MEASUREMENT_ID", System.Environment.GetEnvironmentVariable("FIREBASE_MEASUREMENT_ID"));
    }

    //  Reset variables to dummy values
    static void OnEditorClosed()
    {
        PlayerSettings.SetTemplateCustomValue("FIREBASE_API_KEY", DUMMY_VALUE);
        PlayerSettings.SetTemplateCustomValue("FIREBASE_AUTH_DOMAIN", DUMMY_VALUE);
        PlayerSettings.SetTemplateCustomValue("FIREBASE_PROJECT_ID", DUMMY_VALUE);
        PlayerSettings.SetTemplateCustomValue("FIREBASE_STORAGE_BUCKET", DUMMY_VALUE);
        PlayerSettings.SetTemplateCustomValue("FIREBASE_MESSAGE_SENDER_ID", DUMMY_VALUE);
        PlayerSettings.SetTemplateCustomValue("FIREBASE_APP_ID", DUMMY_VALUE);
        PlayerSettings.SetTemplateCustomValue("FIREBASE_MEASUREMENT_ID", DUMMY_VALUE);
    }
}
