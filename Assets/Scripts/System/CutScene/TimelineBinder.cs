using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.Cutscene
{
    public class TimelineBinder : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _director;

        [SerializeField] private string[] _objectsToBindTags;
        [SerializeField] private string[] _trackNames;

        [SerializeField, ReadOnly] private GameObject[] _objectsToBind;

        private void Awake()
        {
            BindObjects();
        }

        private void BindObjects()
        {
            _objectsToBind = new GameObject[_objectsToBindTags.Length];
            for (int i = 0; i < _objectsToBindTags.Length; ++i)
            {
                _objectsToBind[i] = GameObject.FindGameObjectWithTag(_objectsToBindTags[i]);
            }

            foreach (var playableAssetOutput in _director.playableAsset.outputs)
            {
                for (int i = 0; i < _objectsToBindTags.Length; ++i)
                {
                    if (playableAssetOutput.streamName == _trackNames[i])
                    {
                        _director.SetGenericBinding(playableAssetOutput.sourceObject, _objectsToBind[i]);
                    }
                }
            }
        }
    }
}