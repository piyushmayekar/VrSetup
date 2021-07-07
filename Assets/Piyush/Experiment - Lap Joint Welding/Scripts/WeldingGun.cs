using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace LapWelding
{
    public class WeldingGun : MonoBehaviour
    {
        [SerializeField] Animator _AC;
        [SerializeField] bool isSqueezerTouched = false;
        [SerializeField] Collider squeezerCollider;
        public bool IsHoldingElectrode => currentElectrode != null;

        public event Action OnElectrodePlacedEvent;
        [SerializeField] ParticleSystem ps;
        [SerializeField] GameObject sparkLight;
        [SerializeField] bool isOn = false, isElectrodePlaced = false, isTipInContact = false;
        [SerializeField] GameObject tip;
        [SerializeField] SoundPlayer soundPlayer;
        [SerializeField] GameObject indicator;
        [SerializeField] TMPro.TextMeshProUGUI errorText;
        [SerializeField] ElectrodeType requiredElectrodeType;
        [SerializeField] Electrode currentElectrode;
        [SerializeField] XRSocketInteractor socket_Electrode;

        public ElectrodeType RequiredElectrodeType { get => requiredElectrodeType; set => requiredElectrodeType = value; }
        public bool IsSqueezerTouched { get => isSqueezerTouched; set => isSqueezerTouched = value; }

        public bool IsElectrodePlaced { get => isElectrodePlaced; set => isElectrodePlaced = value; }
        #region SINGLETON
        public static WeldingGun Instance => instance;

        static WeldingGun instance = null;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != null && instance != this)
                Destroy(gameObject);
        }
        #endregion
        public void OnGunHoldStart(SelectEnterEventArgs args)
        {

            // Debug.Log("here socket" + socket_Electrode.socketActive + " squeezerT:" + IsSqueezerTouched);
            //If squeezer is touched & is not holding an electrode, 
            //open the gun & set the e socket active
            if (IsSqueezerTouched)
            {
                IsSqueezerTouched = false;
                ToggleGunSqueezers(true);
                squeezerCollider.enabled = false;
                // Debug.Log("setting " + (!IsHoldingElectrode));
                socket_Electrode.socketActive = !IsElectrodePlaced;

            }
            // Debug.Log("-- " + socket_Electrode.socketActive + " squeezerT:" + IsSqueezerTouched);
            //If squeezer is touched & is holding an electrode, 
            //open the gun & set the e socket inactive, so the electrode drops
        }

        public void OnGunHoldEnd(SelectExitEventArgs args)
        {
            IsSqueezerTouched = false;
            ToggleGunSqueezers(IsSqueezerTouched);
            Invoke(nameof(EnableSqueezerCollider), 1f);
        }

        void EnableSqueezerCollider()
        {
            squeezerCollider.enabled = true;
            IsSqueezerTouched = false;
        }

        public void OnSqueezerHoldStart(HoverEnterEventArgs args)
        {
            if (args.interactor)
            {
                IsSqueezerTouched = true;
            }
        }

        void ToggleGunSqueezers(bool open)
        {
            _AC.SetBool("Open", open);
        }

        public void OnElectrodeSocketEnter(SelectEnterEventArgs args)
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
                ShowErrorIndicator(electrode.ElectrodeType != RequiredElectrodeType, _Constants.ELECTRODE_NOT_CORRECT);
            }
            else
            {
                socket_Electrode.socketActive = false;
                Invoke(nameof(TurnOnElectrodeSocket), 2f);
            }
        }

        public void OnElectrodeSocketExit(SelectExitEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.ELECTRODE_TAG))
            {
                currentElectrode = null;
                IsElectrodePlaced = false;
            }
        }
        void Start()
        {
            ToggleMachine(false);
            ToggleWeldingTip();
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
                sparkLight.SetActive(true);
                soundPlayer.PlayClip(soundPlayer.Clips[0], true);
            }
            else
            {
                ps.Stop();
                sparkLight.SetActive(false);
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


        void TurnOnElectrodeSocket()
        {
            socket_Electrode.socketActive = true;
        }

        /// <summary>
        /// Only turn on welding tip if the appropriate electrode is placed
        /// </summary>
        private void ToggleWeldingTip()
        {
            tip.SetActive(IsElectrodePlaced);
        }

        public void ShowErrorIndicator(bool show, string message = null)
        {
            if (indicator)
            {
                indicator.SetActive(show);
                if (show) errorText.text = message;
            }
        }
    }
}