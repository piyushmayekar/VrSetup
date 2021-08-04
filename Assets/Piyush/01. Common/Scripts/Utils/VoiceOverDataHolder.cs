using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiyushUtils
{
    [CreateAssetMenu(fileName = "VO Data", menuName = "Voice Over Holder", order = 1)]
    public class VoiceOverDataHolder : ScriptableObject
    {
        public List<AudioClip> voiceOvers = new List<AudioClip>();
    }
}