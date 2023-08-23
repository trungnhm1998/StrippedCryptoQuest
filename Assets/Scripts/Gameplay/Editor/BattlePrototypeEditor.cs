using CryptoQuest.Gameplay;
using UnityEngine;

namespace Gameplay.Editor
{
    [UnityEditor.CustomEditor(typeof(BattlePrototype))]
    public class BattlePrototypeEditor : UnityEditor.Editor
    {
        private BattlePrototype Target => target as BattlePrototype;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Give Ability To Character"))
                Target.GiveAbilityToCharacter();
            
            if (GUILayout.Button("Use Ability On Enemy"))
                Target.UseAbilityOnEnemy();
        }
    }
}