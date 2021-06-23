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
        [SerializeField] GameObject psLight;
        [SerializeField] bool isOn = false, isElectrodePlaced = false, isTipInContact = false;
        [SerializeField] GameObject tip;
        [SerializeField] SoundPlayer soundPlayer;
        [SerializeField] ElectrodeType requiredElectrodeType;
        [SerializeField] Electrode currentElectrode;

        public bool IsElectrodePlaced { get => isElectrodePlaced; set => isElectrodePlaced = value; }
        #region SINGLETON
        public static WeldingMachine Instance => instance;

        public ElectrodeType RequiredElectrodeType { get => requiredElectrodeType; set => requiredElectrodeType = value; }

        static WeldingMachine instance = null;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != null && instance != this)
                Destroy(gameObject);
        }
        #endregion

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            ToggleMachine(false);
            ToggleWeldingTip();
            WeldingArea.OnWeldingMachineTipContact += TipInContact;
        }

        public void TipInContact(bool inContact)
        {
            isTipInContact = inContact;
            ToggleMachine(inContact);
        }

        public void ToggleMachine(bool on)
        {
            isOn = on;
            psLight.SetActive(on);
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
        /// <summary>
        /// Called when a task begins.
        /// Sets IsElectrodePlaced to false if the desired electrode type is not currently placed
        /// So as to not turn it on unnecessarily on contact.
        /// </summary>
        /// <param name="type"></param>
        public bool CheckIfRequiredElectrodePlaced(ElectrodeType type)
        {
            IsElectrodePlaced = currentElectrode != null
                && currentElectrode.ElectrodeType == type;
            return IsElectrodePlaced;
        }

        public void OnElectrodePlaced(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.ELECTRODE_TAG))
            {
                Electrode electrode = args.interactable.GetComponent<Electrode>();
                if (electrode.ElectrodeType == RequiredElectrodeType)
                {
                    IsElectrodePlaced = true;
                    currentElectrode = electrode;
                    OnElectrodePlacedEvent?.Invoke();
                }
                ToggleWeldingTip();
            }
        }

        /// <summary>
        /// Only turn on welding tip if the appropriate electrode is placed
        /// </summary>
        private void ToggleWeldingTip()
        {
            tip.SetActive(IsElectrodePlaced);
        }

        public void OnElectrodeRemoved(SelectExitEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.ELECTRODE_TAG))
            {
                IsElectrodePlaced = false;
            }
        }

    }
}

[System.Serializable]
public enum ElectrodeType
{
    _315mm, _4mm
}