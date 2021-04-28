using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlatWelding
{
    public class WeldingMachine : MonoBehaviour
    {
        public event Action OnElectrodePlacedEvent;
        [SerializeField] ParticleSystem ps;
        [SerializeField] bool isOn = false, isElectrodePlaced = false, isTipInContact = false;
        [SerializeField] GameObject tip;

        [SerializeField] SoundPlayer soundPlayer;

        public bool IsElectrodePlaced { get => isElectrodePlaced; set => isElectrodePlaced = value; }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            ToggleMachine(false);
        }

        public void TipInContact(bool inContact)
        {
            isTipInContact = inContact;
            ToggleMachine(inContact);
        }

        public void ToggleMachine(bool on)
        {
            isOn = on;
            if (on && isTipInContact && IsElectrodePlaced)
            {
                ps.Play();
                soundPlayer.PlayClip(soundPlayer.Clips[0], true);
            }
            else
            {
                ps.Stop();
                soundPlayer.StopPlayingAllSounds();
            }
        }

        public void OnElectrodePlaced(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag("Electrode"))
            {
                IsElectrodePlaced = true;
                OnElectrodePlacedEvent?.Invoke();
            }
        }
        public void OnElectrodeRemoved(SelectExitEventArgs args)
        {
            if (args.interactable.CompareTag("Electrode"))
            {
                IsElectrodePlaced = false;
            }
        }
    }
}