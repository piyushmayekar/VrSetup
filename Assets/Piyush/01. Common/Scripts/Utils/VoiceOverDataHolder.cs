using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiyushUtils
{
    [CreateAssetMenu(fileName = "VO Data", menuName = "Voice Over Holder", order = 1)]
    public class VoiceOverDataHolder : ScriptableObject
    {
        [Tooltip("Store data according to the _Language enum index")]
        public List<VOData> voDatas = new List<VOData>();

    }

    [System.Serializable]
    public class VOData
    {
        public List<AudioClip> data = new List<AudioClip>();
    }
}