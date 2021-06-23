using UnityEngine;
using System;
using System.Collections.Generic;

namespace FlatWelding
{
    public class ChippingHammer : MonoBehaviour
    {
        [SerializeField] bool isHittingLeftPoints;
        [SerializeField] SoundPlayer soundPlayer;

        //TODO Remove this
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

        public void PlayHitSound()
        {
            soundPlayer.PlayRandomClip(soundPlayer.ClipLists[0], false);
        }
    }
}