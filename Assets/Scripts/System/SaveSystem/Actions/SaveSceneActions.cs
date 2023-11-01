using IndiGames.Core.SceneManagementSystem.ScriptableObjects;

namespace CryptoQuest.System.SaveSystem.Actions
{
    public class SaveSceneAction : SaveActionBase<SceneScriptableObject>
    {
        public SaveSceneAction(SceneScriptableObject obj): base(obj)
        {
        }
    }

    public class LoadSceneAction : SaveActionBase<SceneScriptableObject>
    {
        public LoadSceneAction(SceneScriptableObject obj) : base(obj)
        {
        }
    }

    public class SaveSceneCompletedAction : SaveCompletedActionBase
    {
        public SaveSceneCompletedAction(bool result) : base(result)
        {
        }
    }

    public class LoadSceneCompletedAction : SaveCompletedActionBase
    {
        public LoadSceneCompletedAction(bool result) : base(result)
        {
        }
    }
}