using UnityEngine;
using System;
using System.Collections.Generic;

namespace FlatWelding
{
    public class ChippingHammer : MonoBehaviour
    {
        [SerializeField] bool isHittingLeftPoints;
        [SerializeField] SoundPlayer soundPlayer;

        public bool IsHittingLeftPoints { get => isHittingLeftPoints; set => isHittingLeftPoints = value; }

        #region SINGLETON
        public static ChippingHammer Instance { get => instance; }
        private static ChippingHammer instance = null;
        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(this);
        }
        #endregion
        internal void PlayHitSound()
        {
            soundPlayer.PlayRandomClip(soundPlayer.ClipLists[0], false);
        }
    }
}