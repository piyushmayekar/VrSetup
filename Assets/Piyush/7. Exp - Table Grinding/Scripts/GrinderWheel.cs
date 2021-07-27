using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Grinding
{
    public class GrinderWheel : MonoBehaviour
    {
        [SerializeField] GrindingMachine grinder;
        [SerializeField] bool isInContact = false;
        [SerializeField] Vector3 contactPoint;
        [SerializeField] Transform sparksT;
        [SerializeField] ParticleSystem _ps;
        [SerializeField] GrindingWheelType type;
        [SerializeField] float warmDownTime = 0.5f;
        [SerializeField] GameObject platesAnimation;
        SoundPlayer soundPlayer;
        public bool IsWheelSpinning => grinder.IsOn;
        public GrindingWheelType Type { get => type; set => type = value; }


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            soundPlayer = GetComponent<SoundPlayer>();
            grinder.OnMachineToggleOff += StopVFX;
        }

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        void OnCollisionEnter(Collision other)
        {
            if (!GrindingMachine.isMachineOn) return;
            isInContact = true;
            contactPoint = other.GetContact(0).point;
            if (!_ps.isPlaying)
                _ps.Play();
            if (!soundPlayer.AudioSource.isPlaying)
                soundPlayer.PlayClip(soundPlayer.Clips[0], true);
            CancelInvoke();
            if (platesAnimation != null)
                platesAnimation.SetActive(false);
        }

        /// <summary>
        /// OnCollisionStay is called once per frame for every collider/rigidbody
        /// that is touching rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        void OnCollisionStay(Collision other)
        {
            contactPoint = other.GetContact(0).point;
            sparksT.position = contactPoint;
        }

        /// <summary>
        /// OnCollisionExit is called when this collider/rigidbody has
        /// stopped touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        void OnCollisionExit(Collision other)
        {
            if (!grinder.IsOn) return;
            isInContact = false;
            contactPoint = Vector3.zero;
            Invoke(nameof(StopVFX), warmDownTime);
        }

        public void StopVFX()
        {
            _ps.Stop();
            soundPlayer.StopPlayingAllSounds();
        }
    }

    [System.Serializable]
    public enum GrindingWheelType
    {
        Rough, Surface
    }
}