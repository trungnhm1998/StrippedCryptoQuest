using CryptoQuest.Events;
using CryptoQuest.System.Dialogue.YarnManager;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.System.Dialogue.Events
{
    public class YarnProjectConfigEvent : ScriptableObject
    {
        public event UnityAction<YarnProjectConfigSO> ConfigRequested;

        /// <summary>
        /// Raises the configure yarn project event for <see cref="DialogRunnerController"/>
        /// </summary>
        /// <param name="yarnProject">The <see cref="YarnProjectConfigSO"/> holding config needed for yarn project</param>
        public void ConfigureYarnProject(YarnProjectConfigSO yarnProject) => ConfigRequested.SafeInvoke(yarnProject);
    }
}