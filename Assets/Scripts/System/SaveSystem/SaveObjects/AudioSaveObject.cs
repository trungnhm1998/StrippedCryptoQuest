using CryptoQuest.Audio;
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.SaveObjects
{
    public class AudioSaveObject : SaveObjectBase<AudioManager>
    {
        public AudioSaveObject(AudioManager obj): base(obj)
        {
        }

        public override string Key => "Audio";

        public override string ToJson()
        {
            return JsonConvert.SerializeObject(RefObject.SaveData);
        }

        public override IEnumerator CoFromJson(string json, Action<bool> callback = null)
        {
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var audioSave = (AudioSave)JsonConvert.DeserializeObject(json);
                    if(audioSave != null)
                    {
                        RefObject.SaveData = audioSave;
                        if (callback != null) { callback(true); }
                        yield break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            if (callback != null) { callback(false); }
            yield break;
        }
    }
}